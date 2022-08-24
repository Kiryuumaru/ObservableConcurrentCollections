using System.ComponentModel;
using System.Threading;
using SynchronizationContextHelpers;
using LockerHelpers;
using ObservableConcurrentCollections.Abstraction;

namespace ObservableConcurrentCollections.Utilities;

/// <summary>
/// Contains all implementations for performing observable operations.
/// </summary>
public abstract class ObservableSyncContext :
    SyncContext,
    ISynchronizedObject
{
    #region Properties

    /// <summary>
    /// Gets the read-write lock for concurrency.
    /// </summary>
    public RWLock RWLock { get; } = new RWLock(LockRecursionPolicy.SupportsRecursion);

    #endregion

    #region Events

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? SynchronizedPropertyChanged;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? UnsynchronizedPropertyChanged;

    /// <summary>
    /// Gets or sets <c>true</c> if the <see cref="PropertyChanged"/> event will invoke on the synchronized context.
    /// </summary>
    public bool SynchronizePropertyChangedEvent { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Raises <see cref="OnPropertyChanged(PropertyChangedEventArgs)"/> with the specified <paramref name="propertyName"/>.
    /// </summary>
    /// <param name="propertyName">
    /// The name of the changed property.
    /// </param>
    protected void OnPropertyChanged(string propertyName)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Invokes <see cref="PropertyChanged"/> and <see cref="SynchronizedPropertyChanged"/>.
    /// </summary>
    /// <param name="args">
    /// The <see cref="PropertyChangedEventArgs"/> event argument.
    /// </param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        if (IsDisposed)
        {
            return;
        }

        UnsynchronizedPropertyChanged?.Invoke(this, args);
        ContextPost(delegate
        {
            SynchronizedPropertyChanged?.Invoke(this, args);
        });

        if (SynchronizePropertyChangedEvent)
        {
            ContextPost(delegate
            {
                PropertyChanged?.Invoke(this, args);
            });
        }
        else
        {
            PropertyChanged?.Invoke(this, args);
        }
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            PropertyChanged = null;
            SynchronizedPropertyChanged = null;
            UnsynchronizedPropertyChanged = null;
        }
        base.Dispose(disposing);
    }

    #endregion
}

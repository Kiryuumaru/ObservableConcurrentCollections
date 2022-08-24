# Observable Helpers

Observable concurrent collection helpers with UI-safe property implementations. Can be used for any MVVM software architectural applications.

**NuGets**

|Name|Info|
| ------------------- | :------------------: |
|ObservableConcurrentCollections|[![NuGet](https://buildstats.info/nuget/ObservableConcurrentCollections?includePreReleases=true)](https://www.nuget.org/packages/ObservableConcurrentCollections/)|

## Installation
```csharp
// Install release version
Install-Package ObservableConcurrentCollections

// Install pre-release version
Install-Package ObservableConcurrentCollections -pre
```
## Supported frameworks
.NET Standard 2.0 and above - see https://github.com/dotnet/standard/blob/master/docs/versions.md for compatibility matrix

## Get Started

All observable events are executed on thread that was used to create the object instance.
To use in UI safe updates, create the object instances at the UI thread or manually configure the ISyncObject.SyncOperation to use UI thread.

## Supports
```csharp
ObservableConcurrentCollection<T>
ObservableConcurrentDictionary<TKey, TValue>
ObservableConcurrentQueue<T>
ObservableConcurrentStack<T>
```

## Usage

### Sample
```csharp
using ObservableConcurrentCollections;

namespace YourNamespace
{
    public class Program
    {
        private ObservableConcurrentCollection<int> collection;

        public void UIThread()
        {
            collection = new ObservableConcurrentCollection<int>(); // Must be created on UI thread to synchronize events

            collection.SynchronizeCollectionChangedEvent = true;

            collection.CollectionChanged += (s, e) =>
            {
                // Executes on UI thread if collection.SynchronizeCollectionChangedEvent is true (default false)
            }
            collection.SynchronizedCollectionChanged += (s, e) =>
            {
                // Executed on UI thread
            }
            collection.UnsynchronizedCollectionChanged += (s, e) =>
            {
                // Executed on current thread
            }
        }
    }
}
```

### Want To Support This Project?
All I have ever asked is to be active by submitting bugs, features, and sending those pull requests down!.

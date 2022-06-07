# UpdateHub
Centralized updates for Unity.

# Why
To reduce individual MonoBehaviour `Update`/'FixedUpdate' calls and coroutines (which are not super fast) by delegate method calls.

### To get called every **Update**
```csharp
var handle = new UpdateHub.UpdateHandle(UpdateHub, System.Action);
```

### To get called in N frames later
```csharp
var handle = new UpdateHub.DelayByFrames(UpdateHub, int N, System.Action);
```

### To get called in N seconds later
```csharp
var handle = new UpdateHub.DelayByTime(UpdateHub, float N, System.Action);
```

### To get called every N frames
```csharp
var handle = new UpdateHub.RepeatByFrames(UpdateHub, int N, System.Action);
```

### To get called every **FixedUpdate**
```csharp
var handle = new UpdateHub.FixedUpdateHandle(UpdateHub, System.Action);
```

### To get called in N seconds (fixed time) later
```csharp
var handle = new UpdateHub.DelayByFixedTime(UpdateHub, float N, System.Action);
```

### To get called every N seconds
```csharp
var handle = new UpdateHub.RepeatByTime(UpdateHub, float N, System.Action);
```

### To get called every N seconds (fixed time)
```csharp
var handle = new UpdateHub.RepeatByFixedTime(UpdateHub, float N, System.Action);
```

### To stop getting called
```csharp
handle.Dispose();
```

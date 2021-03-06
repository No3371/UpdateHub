# UpdateHub
A centralized coroutine implementation for Unity based on Coroutine. Suit for any non-return function that does not rely on update orders.

# Why
- To reduce individual MonoBehaviour `Update` and coroutines (which are not super fast) by delegate method calls to one single Update (System.Action).
- It's handy and clean. I created for work to replace UniRx, check usage below.

# Usage
`UpdateHub` itself does nothing, it relies on external class (ex: MonoBehaviour) to call its `Update` and `FixedUpdate`.

`UpdateHub` belonging to other classes is intended for separation of concerns. If you want something global/singleton, you can just attach it to a global/singleton.

### To get called every **Update**
```csharp
var handle = new UpdateHub.UpdateHandle(UpdateHub, System.Action);
```

### To get called in N frames later
```csharp
var handle = new UpdateHub.DelayByFrames(UpdateHub, int N, System.Action); 
```

### To get called every N frames
```csharp
var handle = new UpdateHub.IntervalByFrames(UpdateHub, int N, System.Action);
```

### To get called every **FixedUpdate**
```csharp
var handle = new UpdateHub.FixedUpdateHandle(UpdateHub, System.Action);
```

### To get called in N seconds (fixed time) later
```csharp
var handle = new UpdateHub.DelayByFixedTime(UpdateHub, float N, System.Action); 
```

### To get called every N seconds (fixed time)
```csharp
var handle = new UpdateHub.IntervalByFrames(UpdateHub, float N, System.Action);
```

### To stop getting called
```csharp
handle.Dispose();
```

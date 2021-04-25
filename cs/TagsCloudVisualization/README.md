# SameSquares
```c#
    for (var i = 0; i < 50; i++)
        layouter.PutNextRectangle(new Size(50, 50));
```
<img src="Images\SameSquares.png">

# FixedSquares
```c#
    var rects = new List<Size> { new Size(30, 30), new Size(40, 40), new Size(50, 50) };
    for (var i = 0; i < 50; i++)
        layouter.PutNextRectangle(rects[i % rects.Count]);
```
<img src="Images\FixedSquares.png">

# ChangingRectangles
```c#
    for (var i = 0; i < 50; i++)
        layouter.PutNextRectangle(new Size(70 + i, 40));
```
<img src="Images\ChangingRectangles.png">
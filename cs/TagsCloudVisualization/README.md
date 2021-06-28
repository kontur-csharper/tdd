```c#
    for (var i = 1; i < 39; i++)
            {
                size = new Size(30 + 2*i, 30 + i);
                rects.Add(layoute.PutNextRectangle(size));
            }
```
<img src="LayoutImage\ImageAt38.png">

```c#
    for (var i = 0; i < 30; i++)
            {
                size = new Size(30, 30);
                rects.Add(layoute.PutNextRectangle(size));
            }
```
<img src="LayoutImage\ImageAt40.png">
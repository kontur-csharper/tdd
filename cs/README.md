# Tag cloud
Облачная кругло-прямоугольная штука

### Картиночки

<div style="display: flex">
  <img src="https://github.com/Folleach/tdd/blob/images/cs/Images/solid.png" width="240" />
  <img src="https://github.com/Folleach/tdd/blob/images/cs/Images/ring.png" width="240" />
  <img src="https://github.com/Folleach/tdd/blob/images/cs/Images/distance.png" width="240" />
</div>

### Штуки всякие
* `ILayouter` - Знает как ставить прямоульнички
* `TagCloud` - Содержит облачную информацию, хочет знать любого ILayouter
* `IVisualizer` - Знает как рисовать, хочет знать любого TagCloud
* `IRender` - Знает куда сохранять, хочет знать любого IVisualizer

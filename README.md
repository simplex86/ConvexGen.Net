## 生成随机凸多边形

简单几句代码即可
```
var convex = new ConvexGenerator(random);

var x = 0;
var y = 0;
var w = 100;
var h = 100;
var c = random.Next(3, 20);

var veritcs = convex.Gen(x, y, w, h, c);
```

## 运行效果
![convex](https://github.com/simplex86/ConvexGen.Net/blob/main/Doc/convex.gif)

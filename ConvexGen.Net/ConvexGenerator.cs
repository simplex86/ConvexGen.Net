using System;
using System.Collections.Generic;
using System.Drawing;

namespace SimpleX
{
    // 生成随机的凸多边形
    // https://kingins.cn/2022/02/18/%E9%9A%8F%E6%9C%BA%E5%87%B8%E5%A4%9A%E8%BE%B9%E5%BD%A2%E7%94%9F%E6%88%90%E7%AE%97%E6%B3%95/
    // 1. 随机生成两个数组X和Y
    // 2. 将X和Y排序
    // 3. 将数组中的最大值和最小值记下来
    // 4. X和Y分别将数组中除最大值最小值的数作为结点随机分成两条正反链
    // 5. X和Y分别计算正反两条链结点之间的差值合成一个数组，得到X-和Y-两个数组
    // 6. 随机将X-和Y-两个数组中的元素组合，构成一个向量数组
    // 7. 将向量按照角度排序
    // 8. 按顺序将他们相连构成凸多边形
    // 9. 将凸多边形平移回原坐标空间
    class ConvexGenerator
    {
        private Random random;

        public ConvexGenerator()
        {
            random = new Random();
        }

        public ConvexGenerator(Random random)
        {
            this.random = random;
        }

        // 生成随机的凸多边形
        public PointF[] Gen(float x, float y, int width, int height, int count)
        {
            var position = new PointF(x, y);
            return Gen(ref position, width, height, count);
        }

        // 生成随机的凸多边形
        public PointF[] Gen(ref PointF position, int width, int height, int count)
        {
            List<int> xlist = new List<int>(count);
            List<int> ylist = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                xlist.Add(random.Next(0, width));
                ylist.Add(random.Next(0, height));
            }
            xlist.Sort();
            ylist.Sort();

            var minX = xlist[0];
            var maxX = xlist[count - 1];
            var minY = ylist[0];
            var maxY = ylist[count - 1];

            List<int> xv = new List<int>(count);
            List<int> yv = new List<int>(count);

            var top = minX;
            var bottom = minX;
            for (int i = 1; i < count - 1; i++)
            {
                var x = xlist[i];
                if (random.Next(0, 10) % 2 == 0)
                {
                    xv.Add(x - top);
                    top = x;
                }
                else
                {
                    xv.Add(bottom - x);
                    bottom = x;
                }
            }
            xv.Add(maxX - top);
            xv.Add(bottom - maxX);

            var left = minY;
            var right = minY;
            for (int i = 1; i < count - 1; i++)
            {
                var y = ylist[i];
                if (random.Next(0, 10) % 2 == 0)
                {
                    yv.Add(y - left);
                    left = y;
                }
                else
                {
                    yv.Add(right - y);
                    right = y;
                }
            }
            yv.Add(maxY - left);
            yv.Add(right - maxY);

            Shuffle(yv);

            var vectors = new List<PointF>(count);
            for (int i = 0; i < count; i++)
            {
                vectors.Add(new PointF(xv[i], yv[i]));
            }
            vectors.Sort((a, b) =>
            {
                var u = Math.Atan2(a.Y, a.X);
                var v = Math.Atan2(b.Y, b.X);

                if (Math.Abs(u - v) <= float.Epsilon) return 0;

                return (u < v) ? -1 : 1;
            });

            var p = new PointF(0, 0);
            var minPolygonX = 0.0f;
            var minPolygonY = 0.0f;
            var points = new List<PointF>(count);
            var result = new List<PointF>(count);

            for (int i = 0; i < count; i++)
            {
                points.Add(p);
                p = p.Sum(vectors[i]);
                minPolygonX = Math.Min(minPolygonX, p.X);
                minPolygonY = Math.Min(minPolygonY, p.Y);
            }

            var translation = new PointF(minX - minPolygonX, minY - minPolygonY);
            for (int i = 0; i < count; i++)
            {
                points[i] = points[i].Sum(translation);
                result.Add(points[i].Sum(position));
            }

            return result.ToArray();
        }

        // Knuth-Durstenfeld Shuffle算法:
        // https://www.jianshu.com/p/762bf1c490a5
        private void Shuffle(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var index = random.Next(0, list.Count - i);
                var tempv = list[index];
                list[index] = list[list.Count - 1 - i];
                list[list.Count - 1 - i] = tempv;
            }
        }
    }

    public static class extent
    {
        public static PointF Sum(this PointF a, PointF b)
        {
            return new PointF()
            {
                X = a.X + b.X,
                Y = a.Y + b.Y,
            };
        }
    }
}

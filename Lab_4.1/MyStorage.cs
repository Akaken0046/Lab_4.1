using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab_4._1
{
    class AObject //Абстракный класс
    {
        public virtual bool CheckPos(int x, int y, MouseButtons mouseButtons) { //проверка позиции
            return false;
        }
        public virtual bool GetUsed() //является ли объект используемый
        {
            return false;
        }
        public virtual bool GetSelected() //является ли объект текущий
        {
            return false;
        }
        public virtual void SetSelected() { } //задать что объект текущий
        public virtual void Draw() { } //отрисовка
    }
    class MyStorage //Хранилище
    {
        int count;
        AObject[] objects;
        public MyStorage(int count)
        {
            this.count = count;
            objects = new AObject[count];
        }
        public int getCount() //получение размера массива
        {
            return count;
        }
        public void setObject(AObject _object) //вставить объект
        {
            for(int i = 0;i < count; i++)
            {
                if(objects[i] == null)
                {
                    objects[i] = _object;
                    return;
                }
            }
        }
        public AObject getObject(int index) //получить объект
        {
            return objects[index];
        }
        public void delObject(int index) //удалить объект
        {
            objects[index] = null;
        }
        public void ActionObject(int x, int y, Graphics graphics,MouseButtons mouseButtons) // действие над объектом
        {
            if (CheckAndUpdate(x, y, mouseButtons)) return;
            if (mouseButtons == MouseButtons.Left)
                setObject(new CCicle(x, y, graphics));
            else
            {
                DelUsed();
            }
        }
        private void DelUsed() //удаление выделенных
        {
            bool flag = false; //нет используемых объектов
            for (int i = 0; i < count; i++)
            {
                if (objects[i] == null) continue;
                if (objects[i].GetUsed())
                {
                    if (objects[i].GetSelected())
                    {
                        for (int j = 0; j < count; j++)
                        {
                            if (objects[j] != null && j != i)
                            {
                                objects[j].SetSelected();
                                break;
                            }
                        }
                    }
                    delObject(i);
                    flag = true;
                }
            }
            if (!flag) //то удаляем текущий
            {
                for (int i = 0; i < count; i++)
                {
                    if (objects[i] == null) continue;
                    if (objects[i].GetSelected())
                    {
                        for (int j = 0; j < count; j++)
                        {
                            if (objects[j] != null && j != i)
                            {
                                objects[j].SetSelected();
                                break;
                            }
                        }
                        delObject(i);
                        return;
                    }
                }
            }
        }
        private bool CheckAndUpdate(int x, int y, MouseButtons mouseButtons) //обновление состояния объектов
        {
            bool tmp = false;
            for (int i = 0; i < count; i++)
            {
                if (objects[i] == null) continue;
                if (objects[i].CheckPos(x, y, mouseButtons))
                {
                    tmp = true;
                    continue;
                }
            }
            return tmp;
        }
        public bool checkObject(int index) //есть ли объект
        {
            if(index >= 0 && index < count)
            {
                return objects[index] != null;
            }
            return false;
        }
    }

    class CCicle : AObject //
    {
        int x,y,r = 15;
        Graphics graphics;
        Pen pen;
        Brush brush;
        bool selected;
        bool used;
        
        public CCicle(int x, int y, Graphics graphics)
        {
            this.x = x;
            this.y = y;
            this.graphics = graphics;
            pen = new Pen(Color.Green);
            pen.Width = 2;
            brush = Brushes.Yellow;
            selected = true;
            used = false;
            Draw();
        }
        public override bool CheckPos(int x,int y, MouseButtons mouseButtons)
        {
            if (Math.Sqrt(Math.Pow((this.x - x), 2) + Math.Pow((this.y - y), 2)) < r*1.5) {
                if (mouseButtons == MouseButtons.Left)
                    selected = true;
                else
                    used = !used;
                return true;
            }
            else
            {
                if(mouseButtons == MouseButtons.Left)
                    selected = false;
                return false;
            }
        }
        public override void Draw()
        {
            if (used)
                brush = Brushes.Green;
            else
                brush = Brushes.Yellow;

            if (selected)
                pen.Color = Color.Blue;
            else
                pen.Color = Color.Black;

            DrawCicle();
        }
        public override bool GetUsed()
        {
            return used;
        }
        public override bool GetSelected()
        {
            return selected;
        }
        public override void SetSelected()
        {
            selected = true;
        }
        private void DrawCicle()
        {
            graphics.FillEllipse(brush, (x - r), (y - r), 2 * r, 2 * r);
            graphics.DrawEllipse(pen, (x - r), (y - r), 2 * r, 2 * r);
        }
    }

    class PaintBox //отрисовка 
    {
        Graphics graphics;
        Bitmap bitmap;
        public PaintBox(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);
            ClearBox();
        }
        public Graphics GetGraphics()
        {
            return graphics;
        }
        public Bitmap GetBitmap()
        {
            return bitmap;
        }
        public void ClearBox() //Очищение 
        {
            graphics.Clear(Color.White);
        }
    }
}

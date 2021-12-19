using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4_ооп
{
    public partial class paintCircles : Form
    {
        MyStorage storage;
        Bitmap bmp = new Bitmap(1000, 1000);
        public paintCircles()
        {
            InitializeComponent();
            storage = new MyStorage();
        }

        private void paintBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (storage.isCheckedStorage(e) == false)//если нажато на пустое место
            {
                storage.AllNotChecked();
                storage.addObject(new CCircle(e.X, e.Y, 50));//добавление нового круга в хранилище
            }
            else//если нажать на круг и нажата ctrl, то можно выделить несколько кругоы
                if (Control.ModifierKeys == Keys.Control)
                    storage.MakeCheckedObjectStorage(e);
                else//если клавиша не нажата, то выделяется только один круг
                {
                    storage.AllNotChecked();
                    storage.MakeCheckedObjectStorage(e);
                }
            this.Refresh();
        }
        private void paintBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);
            storage.DrawAll(paintBox, g, bmp);
        }
        private void paintCircles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Graphics g = Graphics.FromImage(bmp);
                storage.removeCheckedObject();
                g.Clear(Color.White);
            }
        }
    }
    class CCircle
    {
        private int radius;
        private int x;
        private int y;
        private bool isClick;
        public CCircle(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            isClick = true;
        }
        public void Draw(PictureBox pb, Graphics g, Bitmap bmp)//метод, который рисует круг 
        {
            Pen pen;
            if (isClick == true)
                pen = new Pen(Color.Red);
            else
                pen = new Pen(Color.Black);
            g.DrawEllipse(pen, (x - (radius / 2)), (y - (radius / 2)), radius, radius);
            pb.Image = bmp;
        }
        public bool isClicked(MouseEventArgs e)//нажато ли в область круга
        {
            if (((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y)) <= radius * radius)
                return true;
            else
                return false;
        }
        public void MakeClickTrue()
        {
            isClick = true;
        }
        public void MakeClickFalse()
        {
            isClick = false;
        }
        public bool IsClick()
        {
            return isClick;
        }
    }
    class MyStorage//класс хранилища
    {
        int size;
        CCircle[] storage;
        public MyStorage()
        {
            size = 0;
        }
        public void setObject(int i, CCircle obj)
        {
            storage[i] = obj;
        }
        public void addObject(CCircle obj)
        {
            Array.Resize(ref storage, size + 1);
            storage[size] = obj;
            size = size + 1;
        }     
        public bool isCheckedStorage(MouseEventArgs e)//проверяет нажато ли на какой-либо круг
        {
            for (int i = 0; i < size; i++)
                if (storage[i].isClicked(e) == true)
                    return true;
            return false;
        }
        public void MakeCheckedObjectStorage(MouseEventArgs e)//делает объект выделенным, если на него нажато
        {
            for (int i = 0; i < size; i++)
            {
                if (storage[i].isClicked(e) == true)
                {
                    storage[i].MakeClickTrue();
                    i = size;
                }
            }
        }
        public void removeObject(int i)//удаление объекта
        {
            if (size > 1 && i < size)
            {
                CCircle[] storage2 = new CCircle[size - 1];
                for (int j = 0; j < i; j++)
                    storage2[j] = storage[j];
                storage[i] = null;
                for (int j = i; j < size - 1; j++)
                    storage2[j] = storage[j + 1];
                size = size - 1;
                storage = storage2;
            }
            else
            {
                size = 0;
                storage[size] = null;
            }
        }
        public void removeCheckedObject()//удаление выделенных объектов
        {
            for (int i = 0; i < size; i++)
            {
                if (storage[i].IsClick() == true)
                {
                    removeObject(i);
                    i = i - 1;
                }
            }
        }
        public void AllNotChecked()
        {
            for (int i = 0; i < size; i++)
                storage[i].MakeClickFalse();
        }
        public void DrawAll(PictureBox pb, Graphics g, Bitmap bmp)
        {
            for (int i = 0; i < size; i++)
                storage[i].Draw(pb, g, bmp);
        }
        public int getSize()
        {
            return size;
        }
    }
}



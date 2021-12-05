using Boxes_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace User_Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private float _x;
        private float _y;
        private int _amount;

        Manager _manager;
        public MainWindow()
        {
            InitializeComponent();
            _manager = new Manager();
            Amount.MouseDoubleClick += Amount_MouseDoubleClick;
            SurfaceX.MouseDoubleClick += SurfaceX_MouseDoubleClick;
            SurfaceY.MouseDoubleClick += SurfaceY_MouseDoubleClick;
            ShowX.MouseDoubleClick += ShowX_MouseDoubleClick;
            ShowY.MouseDoubleClick += ShowY_MouseDoubleClick;
            BuyX.MouseDoubleClick += BuyX_MouseDoubleClick;
            BuyY.MouseDoubleClick += BuyY_MouseDoubleClick;
        }

        private void BuyY_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BuyY.Clear();
        }

        private void BuyX_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BuyX.Clear();
        }

        private void ShowY_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowY.Clear();
        }

        private void ShowX_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowX.Clear();
        }

        private void Amount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Amount.Clear();
        }

        private void SurfaceY_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SurfaceY.Clear();
        }

        private void SurfaceX_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SurfaceX.Clear();
        }
        private void Supply_BTN(object sender, RoutedEventArgs e)
        {
            if (EmptyFieldsChecker(SurfaceX) == true)
            {
                if (EmptyFieldsChecker(SurfaceY) == true)
                {
                    if (IsItFloatNumber(SurfaceX) == true && IsItFloatNumber(SurfaceY) == true && IsItInteger(Amount) == true)
                    {
                        _x = float.Parse(SurfaceX.Text);
                        _y = float.Parse(SurfaceY.Text);
                        _amount = int.Parse(Amount.Text);  // Emty String Throws Exeption !!! NEED TO FIX IT!!!!
                        result.Text = _manager.Supply(_x, _y, _amount).ToString();

                        SurfaceX.Clear();
                        SurfaceY.Clear();
                        Amount.Clear();
                    }
                }
            }
        }
        private void ShowInfo_BTN(object sender, RoutedEventArgs e)
        {
            if (IsItFloatNumber(ShowX) == true && IsItFloatNumber(ShowY) == true)
            {
                _x = float.Parse(ShowX.Text);
                _y = float.Parse(ShowY.Text);
                string sa = _manager.ShowBoxInfo(_x, _y);
                result.Text = sa;
            }
            ShowX.Clear();
            ShowY.Clear();
        }
        private void Buy_BTN(object sender, RoutedEventArgs e)
        {
            if (EmptyFieldsChecker(BuyX) == true)
            {
                if (EmptyFieldsChecker(BuyY) == true)
                {
                    if (IsItFloatNumber(BuyX) == true && IsItFloatNumber(BuyY) == true) ////////////
                    {
                        _x = float.Parse(BuyX.Text);
                        _y = float.Parse(BuyY.Text);
                        result.Text = _manager.BuyBox(_x, _y).ToString();

                        BuyX.Clear();
                        BuyY.Clear();
                    }
                }
            }
        }
        private bool EmptyFieldsChecker(TextBox insert)
        {
            if (insert.Text == string.Empty)
            {
                result.Text = $"Empty Field: {insert.Text}";
                return false;
            }
            return true;
        }
        private bool IsItFloatNumber(TextBox insert)
        {
            float num = 0;

            if (float.TryParse(insert.Text, out num))
            {
                if (num > 1)
                {
                    return true;
                }
                result.Text = $"It Must To Be Bigger Than: {insert.Text}";
            }
            else
            {
                result.Text = $"Inserted Incorrect Size: {insert.Text}";
            }
            return false;
        }
        private bool IsItInteger(TextBox insert)
        {
            int num = 0;

            if (int.TryParse(insert.Text, out num))
            {
                if (num > 0)
                {
                    return true;
                }
                result.Text = $"Amount Must To Be Bigger Than: {insert.Text}";
            }
            else
            {
                result.Text = $"Amount Must To Be Whole Number: {insert.Text}";
            }
            return false;
        }

        private void BuyX_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}


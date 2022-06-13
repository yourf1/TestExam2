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
using System.Windows.Shapes;
using TextExam2.Model;
using Microsoft.Win32;

namespace TextExam2.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();

            //Вывод в комбобокс типы продуктов 
            TypeProduct.ItemsSource = ClassConnect.com.ProductType.ToList();
        }
        private void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Создал переменную и сразу задал ей значение
                Model.Product NewAddProduct = new Model.Product();
                //Проверка, что все заполнено
                if (String.IsNullOrEmpty(NameProduct.Text) || String.IsNullOrEmpty(PriceProduct.Text) || TypeProduct.SelectedIndex == -1)
                {
                    MessageBox.Show("Заполните все данные!");
                }else{
                    NewAddProduct.ProductName = NameProduct.Text;
                    NewAddProduct.Price = Convert.ToDecimal(PriceProduct.Text);//Т.к цена в децимал
                    NewAddProduct.IdProductType = TypeProduct.SelectedIndex;
                    NewAddProduct.Image = Pach.Text;

                    //Сохраняем в NewAddProduct 
                    ClassConnect.com.Product.Add(NewAddProduct);
                    //Сохраняем данные в БД
                    ClassConnect.com.SaveChanges();
                    MessageBox.Show("Товар успешно добавлен!");
                    this.Close();
                    //Обновляем данные в списке с товаром
                    (this.Owner as ConfigWindow).UpdateProductList();
                    //Обновление после добавления
                    (this.Owner as ConfigWindow).GravDataGrid();
                }
            }catch{
                MessageBox.Show("Непредвидимая ошибка!");
            }
        }

        private void AddImages_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if(op.ShowDialog() == true)
            {
                PhotoAddProd.Source = new BitmapImage(new Uri(op.FileName));
                Pach.Text = op.FileName;
            }
        }
    }
}

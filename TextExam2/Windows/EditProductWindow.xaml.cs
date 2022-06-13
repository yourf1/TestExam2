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
using TextExam2.Windows;

namespace TextExam2.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        // public List<Model.Role> Roles;

        //Обзываем модель чтобы к ней обращаться
        private Model.Product EditProduct;

        public EditProductWindow(Product EditInemProduct)//ПРИНИМАЕМ ПАРАМЕТРЫ
        {
            InitializeComponent();
            try
            {
                TypeProduct.ItemsSource = Model.ClassConnect.com.ProductType.ToList();
                NameProduct.Text = EditInemProduct.ProductName;
                PriceProduct.Text = EditInemProduct.Price.ToString();
                PachImage.Text = EditInemProduct.Image;

                EditProduct = EditInemProduct;
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка!");
            }    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NameProduct.Text) || (String.IsNullOrEmpty(PriceProduct.Text)) || TypeProduct.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не заполнили какой то из элементов!");
            }
            else
            {
                try
                {
                    EditProduct.ProductName = NameProduct.Text;
                    EditProduct.Price = Convert.ToDecimal(PriceProduct.Text);
                    EditProduct.IdProductType = int.Parse(TypeProduct.SelectedValue.ToString());

                    //Сохранение в БД
                    Model.ClassConnect.com.SaveChanges();
                    this.Close();
                    //ОБНОВЛЕНИЕ СПИСКА С ПРОДУКТАМИ, UpdateProductList - СОЗДАЕТСЯ В ConfigWindow
                    (this.Owner as ConfigWindow).UpdateProductList();
                    //Учет фитра после редактирования
                    (this.Owner as ConfigWindow).GravDataGrid();
                }
                catch
                {
                    MessageBox.Show("Непредеднная ошибка");
                }
            }
        }
    }
}
 
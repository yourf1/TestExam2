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


namespace TextExam2.Windows
{
    /// <summary>
    /// Логика взаимодействия для ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        //создаем лист для заполнения из бд для заполнения дата грида
        public List<Model.Product> AllProduct;

        // Лист где будет фильтрация
        public List<Model.Product> FiltrDataGrid;
        public ConfigWindow()
        {
            InitializeComponent();

            //Заполняем комбобокс сортировки типом продуктов
            Sort.ItemsSource = ClassConnect.com.ProductType.ToList(); 

            FulName.Text = Application.Current.Resources["FullName"].ToString();
            var Role = Application.Current.Resources["Role"].ToString();

            //Распределение по ролям
            if(Role == "1")
            {
                EditPtoduct.Visibility = Visibility.Hidden;
                AddProduct.Visibility = Visibility.Hidden;
            }

            if (Role == "2")
            {
                EditPtoduct.Visibility = Visibility.Hidden;
                
            }

            AllProduct = ClassConnect.com.Product.ToList();//В лист переносим данные из таблицы
            ProductList.ItemsSource = AllProduct;

        }

        public void GravDataGrid()
        {
            //Лист для фильтрации заполняем листом с данными
            FiltrDataGrid = AllProduct;

            //сотрировка по категории товара
            if (Sort.SelectedIndex != -1) {
                //В лист для лифьтра записываем те позиции, в которых ид типа продукта равен выбранному ид в комбобоксе
                FiltrDataGrid = FiltrDataGrid.Where(i => i.IdProductType == int.Parse(Sort.SelectedValue.ToString())).ToList();
            }

            //сортировка по цене
            if (Filtr.SelectedIndex != -1)
            {
                if (Filtr.SelectedIndex == 0)
                    FiltrDataGrid = FiltrDataGrid.OrderBy(i => i.Price).ToList();
                if(Filtr.SelectedIndex == 1) 
                    FiltrDataGrid = FiltrDataGrid.OrderByDescending(i => i.Price).ToList();
            } 

            //Лист с продуктами заполняем листом отфильтрованным листом
            ProductList.ItemsSource = FiltrDataGrid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        private void EditPtoduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductList.SelectedItems.Count == 1)// проверка что мы выбрали элемент
            {
                EditProductWindow newWindos = new EditProductWindow(ProductList.SelectedItem as Product);//ОТДАЕМ ПАРАМАТРЫ
                newWindos.Owner = this; // владельцем этим продукт виндов становится конфиг виндов (ЭТО НУЖНО ЧТОБЫ РАБОТАЛО)
                newWindos.Show();
            }
            else
            {
                MessageBox.Show("Не прокатит");
            }
        }
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow NewWindow = new AddProductWindow();
            NewWindow.Owner = this;
            NewWindow.Show();
        }
        //Метод для обновления списка товаров после действий
        public void UpdateProductList()
        {
            AllProduct = ClassConnect.com.Product.ToList();
            ProductList.ItemsSource = AllProduct;
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Сортировка по типу товаров
            GravDataGrid();
        }


        private void Serch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Проверка, что в тесктбоксе больше 1 символа
            if (Serch.Text.Length >= 1)
            {

            }
        }

        private void Filtr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //фильтр по цене
            GravDataGrid();
        }

        private void DelProduct_Click(object sender, RoutedEventArgs e)
        {
            //Проверка, что в DataGrid выбран элемент
            if(ProductList.SelectedItems.Count == 1)
            {
                try
                {
                        //Удаление объекта из DataGrid
                        ClassConnect.com.Product.Remove(ProductList.SelectedItem as Model.Product);
                        ClassConnect.com.SaveChanges();
                        UpdateProductList();
                        MessageBox.Show("Объект успешно удален!");
                }
                catch
                {
                    MessageBox.Show("Произошла непредвиденная ошибка");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UnitConverter
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, List<string>> units = new Dictionary<string, List<string>>();

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация вручную (для C# 7.3)
            units["Длина"] = new List<string> { "Метр", "Километр", "Миля" };
            units["Масса"] = new List<string> { "Килограмм", "Грамм", "Фунт" };
            units["Температура"] = new List<string> { "Цельсий", "Фаренгейт", "Кельвин" };

            categoryComboBox.ItemsSource = units.Keys;
            categoryComboBox.SelectedIndex = 0;
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = categoryComboBox.SelectedItem;
            if (selected == null) return;

            string category = selected.ToString();
            fromUnitComboBox.ItemsSource = units[category];
            toUnitComboBox.ItemsSource = units[category];
            fromUnitComboBox.SelectedIndex = 0;
            toUnitComboBox.SelectedIndex = 1;
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            double input;
            if (!double.TryParse(inputTextBox.Text, out input))
            {
                resultTextBlock.Text = "Введите корректное число.";
                return;
            }

            if (categoryComboBox.SelectedItem == null ||
                fromUnitComboBox.SelectedItem == null ||
                toUnitComboBox.SelectedItem == null)
            {
                resultTextBlock.Text = "Пожалуйста, выберите все параметры.";
                return;
            }

            string category = categoryComboBox.SelectedItem.ToString();
            string from = fromUnitComboBox.SelectedItem.ToString();
            string to = toUnitComboBox.SelectedItem.ToString();

            double result = ConvertUnits(category, from, to, input);
            resultTextBlock.Text = $"Результат: {result:0.###} {to}";
        }

        private double ConvertUnits(string category, string from, string to, double value)
        {
            if (category == "Длина")
                return ConvertLength(from, to, value);
            else if (category == "Масса")
                return ConvertMass(from, to, value);
            else if (category == "Температура")
                return ConvertTemperature(from, to, value);
            else
                return value;
        }

        private double ConvertLength(string from, string to, double value)
        {
            double meters = 0;

            // Преобразуем в метры
            switch (from)
            {
                case "Метр":
                    meters = value;
                    break;
                case "Километр":
                    meters = value * 1000;
                    break;
                case "Миля":
                    meters = value * 1609.34;
                    break;
            }

            // Переводим из метров в целевую единицу
            switch (to)
            {
                case "Метр":
                    return meters;
                case "Километр":
                    return meters / 1000;
                case "Миля":
                    return meters / 1609.34;
                default:
                    return meters;
            }
        }

        private double ConvertMass(string from, string to, double value)
        {
            double kg = 0;

            switch (from)
            {
                case "Килограмм":
                    kg = value;
                    break;
                case "Грамм":
                    kg = value / 1000;
                    break;
                case "Фунт":
                    kg = value * 0.453592;
                    break;
            }

            switch (to)
            {
                case "Килограмм":
                    return kg;
                case "Грамм":
                    return kg * 1000;
                case "Фунт":
                    return kg / 0.453592;
                default:
                    return kg;
            }
        }


        private double ConvertTemperature(string from, string to, double value)
        {
            double celsius = 0;

            switch (from)
            {
                case "Цельсий":
                    celsius = value;
                    break;
                case "Фаренгейт":
                    celsius = (value - 32) * 5 / 9;
                    break;
                case "Кельвин":
                    celsius = value - 273.15;
                    break;
            }

            switch (to)
            {
                case "Цельсий":
                    return celsius;
                case "Фаренгейт":
                    return celsius * 9 / 5 + 32;
                case "Кельвин":
                    return celsius + 273.15;
                default:
                    return celsius;
            }
        }
    }
}
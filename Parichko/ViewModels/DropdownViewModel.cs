using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public class DropdownViewModel : INotifyPropertyChanged
    {
        public List<(string Name, string IconName)> Categories { get; set; }

        private (string Name, string IconName)? selectedCategory;
        public (string Name, string IconName)? SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(SelectedItemName));
                OnPropertyChanged(nameof(SelectedItemImage));
            }
        }

        public string SelectedItemName => SelectedCategory?.Name ?? "Pick an icon";
        public string SelectedItemImage => SelectedCategory?.IconName ?? "placeholder.png";

        public DropdownViewModel()
        {
            Categories = new List<(string, string)>
            {
                ("Cat", "cat.png"),
                ("Dog", "dog.png"),
                ("Fox", "fox.png")
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

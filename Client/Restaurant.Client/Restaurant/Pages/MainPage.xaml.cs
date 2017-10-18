﻿using Autofac;
using ReactiveUI;
using Restaurant.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Restaurant.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	// ReSharper disable once RedundantExtendsListEntry
	public partial class MainPage : MainPageXaml
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<IMainViewModel>();
            Master = new MasterPage();
            var view = App.Container.Resolve<IViewFor<FoodsViewModel>>();
            view.ViewModel = App.Container.Resolve<FoodsViewModel>();
            var page = view as Page;

            Detail = new NavigationPage(page);
        }
    }

    public class MainPageXaml : BaseMasterDetailPage<MainViewModel>
    {
        protected MainPageXaml()
        {
        }
    }

    public class BaseMasterDetailPage<T> : MasterDetailPage, IViewFor<T> where T : class
    {

        public T ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get => ViewModel;

            set => ViewModel = (T)value;
        }

        protected BaseMasterDetailPage()
        {
        }
    }
}

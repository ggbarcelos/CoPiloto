﻿using CoPiloto.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoPiloto.Services.Navigation
{
    public class NavigationService
    {
        static Lazy<NavigationService> LazyNavi = new Lazy<NavigationService>(() => new NavigationService());
        public static NavigationService Current => LazyNavi.Value;

        NavigationService()
        {
            mapeamento = new Dictionary<Type, Type>();
        }

        public async Task PushAsync<TViewModel>(params object[] args) where TViewModel : BaseViewModel
        {
            var page = Locator<TViewModel>(args);

            await Application.Current.MainPage.Navigation.PushAsync(page);
            await (page.BindingContext as BaseViewModel).InitializeAsync(args);
        }
        Page Locator<TViewModel>(object[] args) where TViewModel : BaseViewModel
        {
            var viewModelType = typeof(TViewModel);
            var viewModelTypeName = viewModelType.Name;
            var viewType = VerificarPage(viewModelType);
            Page page;
            if (viewType == null)
            {
                var name = typeof(BaseViewModel).AssemblyQualifiedName.Split('.')[0];

                var viewTypeName = $"{name}.Views.{viewModelTypeName.Substring(0, viewModelTypeName.Length - 9)}Page";
                viewType = Type.GetType(viewTypeName);
                page = Activator.CreateInstance(viewType) as Page;
                CriarMapeamento(page.GetType(), viewModelType);
            }
            else
                page = Activator.CreateInstance(viewType) as Page;


            var viewModel = Activator.CreateInstance(viewModelType);

            if (page != null)
                page.BindingContext = viewModel;

            return page;
        }

        public async Task PopAsync() =>
            await Application.Current.MainPage.Navigation.PopAsync();

        public async Task PopToRootAsync() =>
            await Application.Current.MainPage.Navigation.PopToRootAsync();

        public async Task PushModalAsync<TViewModel>(params object[] args) where TViewModel : BaseViewModel
        {
            var page = Locator<TViewModel>(args);

            await Application.Current.MainPage.Navigation.PushModalAsync(page);
            await (page.BindingContext as BaseViewModel).InitializeAsync(args);
        }

        public async Task PopModalAsync()
            => await Application.Current.MainPage.Navigation.PopModalAsync();

        public Task RemovePage(Type page)
        {
            var listaPagina = new List<Page>();
            var pagina = Application.Current.MainPage.Navigation.NavigationStack;
            foreach (var item in pagina)
            {
                if (item.GetType() == page)
                    listaPagina.Add(item);
            }
            foreach (var item in listaPagina)
                Application.Current.MainPage.Navigation.RemovePage(item);

            return Task.FromResult(true);
        }

        public void SetarMainPage<TViewModel>(object[] args = null) where TViewModel : BaseViewModel
        {
            var page = Locator<TViewModel>(args);
            if (App.Current.MainPage == null)
            {
                App.Current.MainPage = new NavigationPage(page);

                (page.BindingContext as BaseViewModel).InitializeAsync(args);
            }
        }

        protected readonly Dictionary<Type, Type> mapeamento;

        void CriarMapeamento(Type page, Type vm) => 
            mapeamento.Add(vm, page);

        Type VerificarPage(Type vm)
        {
            if (!mapeamento.ContainsKey(vm))
                return null;

            return mapeamento[vm];
        }
    }
}
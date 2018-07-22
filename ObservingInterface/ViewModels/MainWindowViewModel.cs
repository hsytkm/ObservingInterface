using Autofac;
using ObservingInterface.Models;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;

namespace ObservingInterface.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region TextBlock

        private ReactiveProperty<string> _text1 = new ReactiveProperty<string>();
        public ReactiveProperty<string> Text1
        {
            get => _text1;
            set => SetProperty(ref _text1, value);
        }
        private ReactiveProperty<string> _text2 = new ReactiveProperty<string>();
        public ReactiveProperty<string> Text2
        {
            get => _text2;
            set => SetProperty(ref _text2, value);
        }

        #endregion

        #region クラス

        private ObservableCollection<Person> _peopleClass;
        private ObservableCollection<Person> PeopleClass
        {
            get => _peopleClass ?? (_peopleClass = new ObservableCollection<Person>());
        }

        private ReactiveCommand _commandFromClass;
        public ReactiveCommand CommandFromClass
        {
            get => _commandFromClass;
            private set => SetProperty(ref _commandFromClass, value);
        }

        #endregion

        #region インターフェース

        private ObservableCollection<IPerson> _peopleInterface;
        private ObservableCollection<IPerson> PeopleInterface
        {
            get => _peopleInterface ?? (_peopleInterface = new ObservableCollection<IPerson>());
        }

        private ReactiveCommand _commandFromInterface;
        public ReactiveCommand CommandFromInterface
        {
            get => _commandFromInterface;
            private set => SetProperty(ref _commandFromInterface, value);
        }
        
        #endregion

        private static IContainer Container { get; set; }

        public MainWindowViewModel()
        {
            #region クラス

            PeopleClass.Add(new Person());
            PeopleClass.Add(new Person());

            this.Text1.Subscribe(x => PeopleClass[0].SetName(x));
            this.Text2.Subscribe(x => PeopleClass[1].SetName(x));

            // 各テキストボックスに文字があればクリックできる
            this.CommandFromClass = this.PeopleClass
                .ObserveElementProperty(x => x.IsInput)
                .Scan(0, (counter, x) => counter + (x.Value ? 1 : (0 < counter ? -1 : 0)))
                .Select(counter => counter >= this.PeopleClass.Count)
                .ToReactiveCommand();
            this.CommandFromClass
                .Subscribe(x => Debug.WriteLine("Click CommandFromClass"));

            #endregion

            #region インターフェース

            // DIコンテナ
            var builder = new ContainerBuilder();
            builder.RegisterType<Person>().As<IPerson>();
            Container = builder.Build();
            using (var scope = Container.BeginLifetimeScope())
            {
                PeopleInterface.Add(scope.Resolve<IPerson>());
                PeopleInterface.Add(scope.Resolve<IPerson>());
            }

            this.Text1.Subscribe(x => PeopleInterface[0].SetName(x));
            this.Text2.Subscribe(x => PeopleInterface[1].SetName(x));

            // 各テキストボックスに文字があればクリックできる
            //   ★インターフェースを使ってクラスへの依存性をなくしている場面で、
            //     どのように実装すれば良いのでしょうか？
            //   ★追記：IPerson に INotifyPropertyChanged を継承させた
            //           okazuki様、ありがとうございます
            this.CommandFromInterface = this.PeopleInterface
                .ObserveElementProperty(x => x.IsInput)
                .Scan(0, (counter, x) => counter + (x.Value ? 1 : (0 < counter ? -1 : 0)))
                .Select(counter => counter >= this.PeopleClass.Count)
                .ToReactiveCommand();
            this.CommandFromInterface
                .Subscribe(x => Debug.WriteLine("Click CommandFromInterface"));

            #endregion
        }
    }
}

# ObservingInterface

■やりたいこと
 ObservableCollection内の全てインスタンスのフィールドフラグが true だった場合に、
 UIボタンを押せるようにしたい。

■困っていること
 ObservableCollectionのインスタンスは、依存性を下げるためDIコンテナ(Autofac)を使って生成していますが、
 ObservableCollection<Interface> だと、インスタンスのフィールドを参照できないのでビルドが通りません。

 ObservableCollection<Class> にすれば、動作するコードを書けますが、
 クラスに依存してしまうので、DIコンテナを使ってる意味がなくなるように思います。

 依存性を下げつつ、やりたいことを実現するには、どうすれば良いでしょうか…？

 各インスタンスの観察は、ObserveElementPropertyとScanで実現しています。
 https://gist.github.com/emoacht/9de6b5780838b6d04c06

以上

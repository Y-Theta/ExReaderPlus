## <font color="007acc">**ExReaderPlus**</font>

##### <font color="007acc">概述</font>


这是一款生产实习时开发的UWP平台英语阅读软件，主要功能有下：
* 导入和浏览英文文章
* 通过选择词库，在英文文章中将词库中的单词标注出来，并可以选择是否已掌握相关单词
* 将阅读中碰到的单词添加到词库中
* 浏览词库，在词库中查看单词的掌握状态
* 新建词库

这款软件作为生产实习的产物，在一些细节方面的考虑应该比较欠缺，代码也不够简洁，适合初学者参考。

##### <font color="007acc">预览</font>

![首页](http://wx2.sinaimg.cn/large/73421b88ly1fzkw1ccfq2j20tm0kcgma.jpg)
<p align="center"><font size="2">首页</font></p>

![首页](http://wx2.sinaimg.cn/large/73421b88ly1fzkw1d3du7g20tk0ka1ky.gif)
![首页](http://wx4.sinaimg.cn/large/73421b88ly1fzkw1diblrg20tk0ka4cv.gif)
<p align="center"><font size="2">阅读界面</font></p>

![首页](http://wx3.sinaimg.cn/large/73421b88ly1fzkw1eto6pg20tk0kakjm.gif)
<p align="center"><font size="2">单词界面</font></p>

##### <font color="007acc">技术</font>

因为接触UWP也没有很久，所以用到的都是一些很基本的层面：
* 左边的划出菜单和右边的划出菜单使用了UWP的自定义模板化控件，是自定义实现的非原生控件
* 富文本编辑器是继承自Richtextbox的模板化控件
* 单词页使用了继承自panel的自定义布局
* 划出菜单书架的图标是通过给Segoe字体增加字形得到的
* 单词的鼠标效果是通过计算位置后控件覆盖做到的
* 采用的是MVVM的方式
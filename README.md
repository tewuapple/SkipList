SkipList
========

> Skip lists are a data structure that can be used in place of balanced trees. Skip lists use probabilistic balancing rather than strictly enforced balancing and as a result the algorithms for insertion and deletion in skip lists are much simpler and significantly faster than equivalent algorithms for balanced trees.

跳表（skiplist）是一个非常优秀的数据结构，实现简单，插入、删除、查找的复杂度均为O(logN)。LevelDB的核心数据结构是用跳表实现的，redis的sorted set数据结构也是有跳表实现的。

其结构如下所示：

![](http://h.hiphotos.baidu.com/album/whcrop%3D470%2C110%3Bq%3D90/sign=8a9e871b3bf33a879e385658a92c2d0c/9c16fdfaaf51f3deeb28bd4f95eef01f3a297906.jpg)

所有操作均从上向下逐层查找，越上层一次next操作跨度越大。其实现是典型的空间换时间。

具体的细节，可参考维基百科[http://en.wikipedia.org/wiki/](http://en.wikipedia.org/wiki/)
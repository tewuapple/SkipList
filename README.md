SkipList
========

> Skip lists are a data structure that can be used in place of balanced trees. Skip lists use probabilistic balancing rather than strictly enforced balancing and as a result the algorithms for insertion and deletion in skip lists are much simpler and significantly faster than equivalent algorithms for balanced trees.

跳表（skiplist）是一个非常优秀的数据结构，实现简单，插入、删除、查找的复杂度均为O(logN)。LevelDB的核心数据结构是用跳表实现的，redis的sorted set数据结构也是有跳表实现的。

其结构如下所示：

![](http://tewuapple.github.com/images/github/SkipList.png)

所有操作均从上向下逐层查找，越上层一次next操作跨度越大。其实现是典型的空间换时间。

具体的细节，可参考维基百科[http://en.wikipedia.org/wiki/Skiplist](http://en.wikipedia.org/wiki/Skiplist)

Licenses
--------
- LCSDiff is licensed under the terms of the Apache 2.0 License. The license can be found at
  http://www.apache.org/licenses/LICENSE-2.0

Donate:
--------
<a href="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&amp;hosted_button_id=NVTPEJBETH7NL"><img src="http://images.cnblogs.com/cnblogs_com/tewuapple/472134/o_o_donate-with-paypal.png" border="0" alt=""></a>
 <a href='http://me.alipay.com/tewuapple'> <img src='http://images.cnblogs.com/cnblogs_com/tewuapple/472134/o_o_donate-with-alipay.png' /> </a>


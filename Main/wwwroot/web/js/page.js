//路由 对象
var hzyRouter = {
    settings: {
        //路由触发 和加载 事件
        callback: null
    },
    //加载路由
    load: function (text, href) {
        if (href.slice(0, 2) != "#!") {
            href = "#!" + escape(text) + "#!" + href;
        }
        top.window.location.hash = href;
    },
    //初始化
    init: function () {
        window.addEventListener('load', function () {
            var hash = top.window.location.hash;
            if (hash.slice(0, 2) != "#!") return;
            var obj = hzyRouter.analysisHash(hash);
            if (hzyRouter.settings.callback) hzyRouter.settings.callback(unescape(obj.text), obj.href, obj.parm);
        }, false);
        window.addEventListener('hashchange', function () {
            var hash = top.window.location.hash;
            if (hash.slice(0, 2) != "#!") return;
            var obj = hzyRouter.analysisHash(hash);
            if (hzyRouter.settings.callback) hzyRouter.settings.callback(unescape(obj.text), obj.href, obj.parm);
        }, false);
    },
    //解析路由
    analysisHash: function (href) {
        //解析路由

        var site = href.split('#!');

        href = "#!" + site[2];

        var text = site[1];

        if (href.indexOf('#!') >= 0) {
            href = href.slice(2) || '/';
        } else {
            href = href || '/';
        }

        var parm = {};

        if (href.indexOf('/?') >= 0) {
            //检测是否有参数 /#!/User/Index/?id=1
            var _index = href.indexOf('/?');
            var _parm = href.substr(_index, href.length - _index).slice(2);

            if (_parm.indexOf('&') > 0) {
                var array = _parm.split('&');
                for (var i = 0; i < array.length; i++) {
                    var _arr = array[i].split('=');
                    parm[_arr[0]] = _arr[1];
                }
            } else {
                var _arr = _parm.split('=');
                parm[_arr[0]] = _arr[1];
            }

            href = href.substring(0, _index);
        }

        return {
            key: href,
            text: text,
            parm: parm
        };
    }
};

//网站类
var web = {
    init: function () {
        hzyRouter.init();
        //路由事件
        hzyRouter.settings.callback = function (t, h, p) {
            //t:标题 h:链接地址 p:参数
            var hash = top.window.location.hash;
            var arry = hash.split("#!");
            $('title').text(t);
            $(".hzy-content").load(arry[2], function () {
                $(".navbar-nav a").removeClass("active");
                $("a[hzy-router-href='"+arry[2]+"']").addClass('active');
                console.log('加载' + t);
            });
        }
        $("#hzy-container").on("click", "[hzy-router-href]", function () {
            var href = $(this).attr("hzy-router-href");
            var text = $(this).attr("hzy-router-text");
            hzyRouter.load(text, href);
        });

    }

};
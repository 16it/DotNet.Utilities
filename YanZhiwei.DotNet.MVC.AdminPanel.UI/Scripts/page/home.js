/// <reference path="../jquery.easyui-1.4.5.js" />
var Home = function () {
    var loadEasyUIMenu = function () {
        $("#RightAccordion").accordion({ //初始化accordion
            fillSpace: true,
            fit: true,
            border: false,
            animate: false
        });

        $.post("/Home/GetTreeByEasyui", { "id": "0" }, function (data) {
            if (data == "0") {
                window.location.href = '/Login/Index';
            }

            $.each(data, function (i, e) {
                var id = e.id;
                $('#RightAccordion').accordion('add', {
                    title: e.text,
                    content: "<ul id='tree" + id + "' ></ul>",
                    selected: true,
                    iconCls: e.iconCls
                });
            });
        }, "json");
    }

    return {
        init: function () {
            loadEasyUIMenu();
        },
        refreshTab: function () {
            //var index = $('#tabs').tabs('getTabIndex', $('#tabs').tabs('getSelected'));
            //if (index != -1) {
            //    var tab = $('#tabs').tabs('getTab', index);
            //    $('#tabs').tabs('update', {
            //        tab: tab,
            //        options: {
            //            selected: true
            //        }
            //    });
            //}
        },
        closeTab: function () {
            //$('.tabs-inner span').each(function (i, n) {
            //    var t = $(n).text();
            //    if (t != '') {
            //        if (t != "我的主页") {
            //            $('#tabs').tabs('close', t);
            //        }
            //    }
            //});
        }
    }
}();
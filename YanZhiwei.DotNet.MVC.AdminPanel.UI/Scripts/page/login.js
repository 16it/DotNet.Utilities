/// <reference path="../jsUtils.js" />
/// <reference path="../jquery.easyui-1.4.5.js" />
/// <reference path="../easyuiUtils.js" />

var Login = function () {
    var hanlderUISubscribe = function () {
        /// <summary>
        /// 处理UI元素事件订阅
        /// </summary>
        $("#loginDialog").dialog({
            title: "用户登录",
            closable: false,
            iconCls: 'icon-user_b',
            modal: true,
            width: 310,
            height: 220,
            buttons: [{
                id: "loginBtn",
                text: "登 录",
                handler: function () {
                    if ($("#loginFrm").form('validate')) {
                        hanlderLogin();
                    }
                }
            }]
        });

        $('#imgValiCode').click(function () {
            $(this).attr('src', '/Login/GetValidatorGraphics?time=' + jsUtils.datetime.now());
        });
    }
    var hanlderLogin = function () {
        /// <summary>
        /// 处理登陆
        /// </summary>
        var _validateCode = $('#txtValidateCode').val(),
            _userName = $('#txtName').val(),
            _userPwd = $('#txtPwd').val();
        if (checkedValidateCode(_validateCode)) {
            var _postData = {
                AccountName: _userName,
                Password: _userPwd
            };
            //异步实现登录功能
            $.post("/Login/UserLogin", _postData, function (data) {
                if (data === "OK") {
                    window.location.href = "/Home/Index";
                }
                else {
                    alert(data);
                    window.location.href = "/Login/Index/";
                }
            });
        }
    }
    var checkedValidateCode = function (validateCode) {
        /// <summary>
        /// 处理验证码判断
        /// </summary>
        validateCode = $.trim(validateCode).toUpperCase();
        if (validateCode === '') {
            $.show_alert("信息", "验证码不能为空！");
            return false;
        }
        var _cookie = jsUtils.cookie.read('ValidatorCode').toUpperCase();
        if (validateCode !== _cookie) {
            $.show_alert("信息", "验证码不一致！");
            $("#imgValiCode").attr('src', '/Login/GetValidatorGraphics?time=' + jsUtils.datetime.now());
            return false;
        }
        return true;
    }
    return {
        init: function () {
            hanlderUISubscribe();
            $('#txtName').val('admin');
            $('#txtPwd').val('admin');
            $('#txtValidateCode').val(jsUtils.cookie.read('ValidatorCode'));
        }
    }
}();
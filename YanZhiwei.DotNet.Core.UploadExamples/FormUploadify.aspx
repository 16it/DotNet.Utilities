<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormUploadify.aspx.cs" Inherits="YanZhiwei.DotNet.Core.UploadExamples.FormUploadify" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Uploadify</title>
    <link href="Scripts/uploadify/uploadify.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/uploadify/jquery.uploadify.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#uploadify").uploadify({
                'uploader': 'BackHandler/FileUpload.ashx',
                swf: 'Scripts/uploadify/uploadify.swf',
                'buttonText': '上传图片',
                'cancelImg': 'Scripts/uploadify/uploadify-cancel.png',
                'queueID': 'fileQueue',
                'auto': false,
                'multi': true
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="fileQueue"></div>
        <input type="file" name="uploadify" id="uploadify" />
        <p>
            <a href="javascript:$('#uploadify').uploadify('upload')">上传</a>| 
            <a href="javascript:$('#uploadify').uploadify('cancel')">取消上传</a>
        </p>
    </form>
</body>
</html>
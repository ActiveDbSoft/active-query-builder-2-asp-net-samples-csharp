<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserFields.ascx.cs" Inherits="Samples.UserFields" %>
<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>

<style>
    #expression {
        width: 100%;
        max-width: 100%;
        border: 1px solid #cccccc;
        box-sizing: border-box;
        padding: 4px;
        border-radius: 4px;
    }

    .dialog-form input {
        width: 100%;
        border: 1px solid #cccccc;
        box-sizing: border-box;
        padding: 4px;
        border-radius: 4px;
    }
</style>

<AQB:QueryBuilderControl Language="en" OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server" OnInit="QueryBuilderControl1_Init" EnableUserFields="True" OnSQLUpdated="QueryBuilderControl1_OnSQLUpdated" />
<div id="all">
    <div id="content-container">
        <div id="qb-ui">
            <AQB:ObjectTreeView ID="ObjectTree1" runat="server" />
            <div id="center">
                <AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
                <div class="qb-ui-canvas-container block-flat">

                    <AQB:Canvas ID="Canvas1" runat="server" />
                    <AQB:StatusBar ID="StatusBar1" runat="server" />
                    <AQB:Grid ID="Grid1" UseCustomExpressionBuilder="AllColumns" runat="server" />
                </div>
            </div>

            <div class="clear">
            </div>
        </div>
    </div>
    <AQB:SqlEditor ID="SQLEditor1" runat="server"></AQB:SqlEditor>
    <div style="margin-right: 10px">
        <div class="ui-dialog ui-widget sql block-flat-transparent" style="position: relative; width: 100%">
            <div class="ui-dialog-titlebar ui-widget-header" style="margin-bottom: 0; border: 0;">
                <span class="ui-dialog-title-dialog">Query with user expressions</span>
            </div>
            <div class="ui-dialog-content ui-widget-content" id="user-fields-sql" style="width: 100%">
                <textarea style="width: 100%; height: 200px"></textarea>
            </div>
        </div>
    </div>
</div>

<script>
    $(document).ready(function () {

        QB.Web.Core.bind(QB.Web.Core.Events.UserDataReceived, function (e, data) {
            $('#user-fields-sql textarea').val(data);
        });

        QB.Web.Application.Canvas.on(QB.Web.Canvas.Events.CanvasOnAddUserField, function (e, obj) {
            var form = $('<div class="dialog-form" title="Add user field">');

            form.append('<label for="fieldName">Name</label>');
            form.append($('<input type="text" id="fieldName">'));

            form.append('<label for="expression">expression</label>');
            form.append($('<textarea id="expression"></textarea>'));
            form.append('<span id="validation" style="color: red"></span>');

            var dialog = form.dialog({
                autoOpen: false,
                height: 250,
                width: 300,
                modal: true,
                buttons: {
                    "Validate": function () {
                        QB.Web.Application.validateExpression($('#expression').val(), function (valid, error) {
                            if (valid)
                                $('#validation').text('OK');
                            else $('#validation').text(error);

                            console.log(error);
                        });
                    },
                    "Add": function () {
                        var fieldName = $('#fieldName').val();
                        var expression = $('#expression').val();
                        obj.addUserField(fieldName, expression);
                        dialog.dialog("close");
                    },
                    Cancel: function () {
                        dialog.dialog("close");
                    }
                },
                close: function () {
                    form.remove();
                }
            });

            dialog.dialog("open");
        });

        QB.Web.Application.Canvas.on(QB.Web.Canvas.Events.CanvasOnEditUserField, function (e, obj) {
            var form = $('<div class="dialog-form" title="Edit virtual field">');

            form.append('<label for="fieldName">Name</label>');
            form.append($('<input type="text" id="fieldName" value="' + obj.fieldName + '">'));

            form.append('<label for="expression">expression</label>');
            form.append($('<textarea type="text" id="expression">' + obj.expression + '</textarea>'));
            form.append('<span id="validation" style="color: red"></span>');

            var dialog = form.dialog({
                autoOpen: false,
                height: 250,
                width: 300,
                modal: true,
                buttons: {
                    "Validate": function () {
                        QB.Web.Application.validateExpression($('#expression').val(), function (valid, error) {
                            if (valid)
                                $('#validation').text('OK');
                            else $('#validation').text(error);

                            console.log(error);
                        });
                    },
                    "Edit": function () {
                        var fieldName = $('#fieldName').val();
                        var expression = $('#expression').val();
                        obj.editUserField(fieldName, expression);
                        dialog.dialog("close");
                    },
                    Cancel: function () {
                        dialog.dialog("close");
                    }
                },
                close: function () {
                    form.remove();
                }
            });

            dialog.dialog("open");
        });

        QB.Web.Application.Grid.on(QB.Web.Grid.Events.GridBeforeCustomEditCell, function (e, obj) {
            var str;
            if (obj.columnType === MetaData.FieldParamType.condition)
                str = 'condition';
            else str = 'expression';

            var form = $('<div class="dialog-form" title="Add ' + str + '">');
            var input = $('<input type="text" value="' + (obj.value || '') + '">');
            form.append(input);
            form.append('<span id="validation" style="color: red"></span>');

            var dialog = form.dialog({
                autoOpen: false,
                height: 160,
                width: 300,
                modal: true,
                buttons: {
                    "Validate": function () {
                        if (obj.columnType === MetaData.FieldParamType.condition)
                            QB.Web.Application.validateCondition(input.val(), function (valid, error) {
                                if (valid)
                                    $('#validation').text('OK');
                                else $('#validation').text(error);

                                console.log(error);
                            });
                        else
                            QB.Web.Application.validateExpression(input.val(), function (valid, error) {
                                if (valid)
                                    $('#validation').text('OK');
                                else $('#validation').text(error);

                                console.log(error);
                            });
                    },
                    "Add value": function () {
                        obj.cell.updateValue(input.val());
                        dialog.dialog("close");
                    },
                    Cancel: function () {
                        dialog.dialog("close");

                    }
                },
                close: function () {
                    form.remove();
                }
            });

            dialog.dialog("open");
        });
    });
</script>

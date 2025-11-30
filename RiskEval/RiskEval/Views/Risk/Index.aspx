<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<RiskEval.Models.RiskRequestViewModel>" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Risk Analysis – Sample JSON Caller</title>
    <style type="text/css">
        textarea { width: 100%; height: 200px; font-family: Consolas, monospace; font-size: 11px; }
        .container { width: 960px; margin: 20px auto; }
        .label { font-weight: bold; }
    </style>
    <script type="text/javascript">
        function JsonChange() {
            if (document.getElementById('selectedKey').value == 'flood') {
                document.getElementById('txtRequest').value = document.getElementById('txtFlood').value;
            }
            else {
                document.getElementById('txtRequest').value = document.getElementById('txtWildfire').value;
            }
        }
    </script>
</head>
<body>
<div class="container">
    <h2>Risk Analysis Sample – Legacy .NET MVC Caller</h2>

    <p>Webhook URL: <strong><%= Model.WebhookUrl%></strong></p>

    <% using (Html.BeginForm("Send", "Risk", FormMethod.Post))
       { %>
        <div>
            <span class="label">Select sample JSON payload:</span><br />
            <select id="selectedKey" name="selectedKey" onchange="JsonChange();">
                <% foreach (var key in Model.AvailableKeys)
                   { %>
                    <option value="<%= key %>" <%= key == Model.SelectedKey ? "selected=\"selected\"" : "" %>><%= key%></option>
                <% } %>
            </select>
            <input type="submit" value="Send to n8n Webhook" />
        </div>
    <% } %>

    <hr />

    <h3>Request JSON</h3>
    <textarea id='txtRequest' readonly="readonly"><%= Model.RequestJson%></textarea>
    <textarea id='txtWildfire' style="display:none" readonly="readonly"><%= Model.WildfireJson%></textarea>
    <textarea id='txtFlood' style="display:none" readonly="readonly"><%= Model.FloodJson%></textarea>

    <h3>Response JSON</h3>
    <textarea readonly="readonly"><%= Model.ResponseJson%></textarea>
</div>
</body>
</html>
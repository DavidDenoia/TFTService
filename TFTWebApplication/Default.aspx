<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TFTWebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Conversor de Números</h1>

    <div class="form-group">
        <label for="txtNumero">Introduce un número:</label>
        <asp:TextBox ID="txtNumero" CssClass="form-control" runat="server"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:Button ID="Button1" runat="server" Height="28px" OnClick="Traducir_Click" Text="Traducir" Width="118px" />
    </div>

    <br />

    <asp:Label ID="lblResultado" runat="server" ForeColor="Blue" CssClass="lead"></asp:Label>

    <asp:Repeater ID="rptResultados" runat="server">
        <ItemTemplate>
            <div class="card mt-3">
                <div class="card-body">
                    <h5 class="card-title"><strong>Tipo:</strong> <%# Eval("Tipo") %></h5>
                    <p class="card-text"><strong>Respuesta:</strong> <%# string.Join(", ", (string[])Eval("Respuestas")) %></p>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>

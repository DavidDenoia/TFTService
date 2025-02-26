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
                    <!--Titulo de la tarjeta-->
                    <h5 class="card-title">
                        <strong><%# Eval("Tipo") %></strong>
                       <asp:LinkButton ID="botonExpandir" runat="server" CssClass="btn btn-link toggle-btn float-right">
                            <i class="bi bi-chevron-down"></i>
                       </asp:LinkButton>



                    </h5>

                    <!--Respuesta inicial-->
                    <p class="card-text">
                        <asp:LinkButton runat="server" CommandArgument='<%# string.Join(", ", (string[])Eval("Respuestas")) %>' 
                            OnClick="Leer_Click"
                            CssClass="btn btn-lg me-1">
                                <i class="bi bi-volume-up"></i>
                        </asp:LinkButton>

                        <asp:LinkButton runat="server" CommandArgument='<%# string.Join(", ", (string[])Eval("Respuestas")) %>'
                            OnClick="Copiar_Click"
                            CssClass="btn btn-lg me-1">
                            <i class="bi bi-copy"></i>
                        </asp:LinkButton>

                        <strong>Respuesta:</strong> <%# string.Join(", ", (string[])Eval("Respuestas")) %>

                    </p>

                    <!--Contenido expansible-->
                     <div id='<%# "content" + Container.ItemIndex %>' class="collapse">


                        <!-- Valor Numérico -->
                        <%# !string.IsNullOrEmpty((string)Eval("ValorNumerico")) ? $"<p><strong>{Eval("TitValorNumerico")}</strong>: {Eval("ValorNumerico")}</p>" : "" %>

                        <!--Opciones adicionales-->
                        <h6><%# Eval("TitOpciones") %></h6>
                        <ul>
                                <%# string.Join("", ((TFTWebApplication.ServiceReference1.Opcion[])Eval("MasOpciones"))
                                .SelectMany(o => o.Opciones)
                                .Select(opcion => $"<li>{opcion}</li>")) %>
                        </ul>

                        <!--Ejemplos-->
                        <h6><%# Eval("TitEjemplos") %></h6>
                        <ul>
                           <%# string.Join("", ((string[])Eval("Ejemplos")).Select(ejemplo => $"<li>{ejemplo}</li>")) %>
                        </ul>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>

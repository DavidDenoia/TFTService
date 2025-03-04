<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TFTWebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Conversor de Números</h1>

   

    <asp:Panel ID="Panel1" runat="server" DefaultButton="botonTraducir">
    <div class="container mt-3">
        <!-- Título -->
        <div class="text-center border rounded-top py-2 bg-light">
            <strong class="text-primary">
                <asp:Label ID="lblTitulo" runat="server" Text="Números a letras. Números a texto"></asp:Label>
            </strong>
        </div>

        <!-- Campo de búsqueda -->
        <div class="border rounded-bottom p-3 bg-white">
            <div class="input-group">
                <asp:TextBox ID="txtNumero" CssClass="form-control flex-grow-1" MaxLength="130" 
                    placeholder="Escriba un número con cifras o un número romano" runat="server"></asp:TextBox>

                <asp:LinkButton ID="botonLimpiar" runat="server" CssClass="btn btn-link text-primary" OnClick="Limpiar_Click">
                    <i class="bi bi-x-lg"></i>
                </asp:LinkButton>

                <asp:LinkButton ID="botonTraducir" runat="server" CssClass="btn btn-link text-primary" OnClick="Traducir_Click">
                    <i class="bi bi-search"></i>
                </asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Panel>





    <br />

    <asp:Label ID="lblResultado" runat="server" ForeColor="Blue" CssClass="lead"></asp:Label>

    <asp:Repeater ID="rptResultados" runat="server">
        <ItemTemplate>
            <div class="card mt-3">
                <div class="card-body">
                    <!--Titulo de la tarjeta-->
                   
                    <div class="d-flex justify-content-between align-items-center">
                        <h5 class="card-title mb-0">
                            <strong><%# Eval("Tipo") %></strong>
                        </h5>

                        <!--Botones derecha-->
                        <div class="d-flex align-items-center gap-0">
                            <asp:LinkButton ID="botonNotas" runat="server" CssClass="btn btn-link" data-bs-toggle="modal" 
                                data-bs-target='<%# "#notasModal" + Container.ItemIndex %>' title='<%# Eval("TitNotas") %>'>
                                <i class="bi bi-clipboard"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="botonReferencias" runat="server" CssClass="btn btn-link"  data-bs-toggle="modal" 
                                data-bs-target='<%# "#referenciasModal" + Container.ItemIndex %>' title='<%# Eval("TitReferencias") %>'>
                                 <i class="bi bi-file-earmark-text"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-link toggle-btn"
                                OnClientClick='<%# "Expandir(\"content" + Container.ItemIndex + "\", this); return false;" %>'>
                                 <i class="bi bi-chevron-down"></i>
                            </asp:LinkButton>
                        </div> 
                    </div>


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
                        <%# !string.IsNullOrEmpty((string)Eval("ValorNumerico")) ? $"<h6 class=\"text-primary\">{Eval("TitValorNumerico")}: {Eval("ValorNumerico")}</h6>" : "" %>

                        <!--Opciones adicionales-->
                        <h6 class="text-primary"><%# Eval("TitOpciones") %></h6>
                        <ul>
                            <%# string.Join("", ((TFTWebApplication.ServiceReference1.Opcion[])Eval("MasOpciones"))
        .Select(o => $"<strong>{o.Titulo}:</strong><ul>{string.Join("", o.Opciones.Select(op => $"<li>{op}</li>"))}</ul>")) %>
                        </ul>

                        <!--Ejemplos-->
                        <h6 class="text-primary"><%# Eval("TitEjemplos") %></h6>
                        <ul>
                      
                             <%# (Eval("Ejemplos") as string[] ?? new string[0]).Length > 0 
                                    ? string.Join("", (Eval("Ejemplos") as string[]).Select(ejemplo => $"<li>{ejemplo}</li>"))
                                        : "<li>No hay ejemplos disponibles.</li>" %>
                        </ul>
                    </div>
                </div>

                <!--Notas-->
                <div class="modal" id='<%# "notasModal" + Container.ItemIndex %>' tabindex="-1">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title text-primary"><%# Eval("TitNotas") %></h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" arial-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <ul>
                                    <%# (Eval("Notas") as string[] ?? new string[0]).Length > 0 
                                            ? string.Join("", (Eval("Notas") as string[]).Select(nota => $"<li>{nota}</li>"))
                                            : "<li>No hay notas disponibles.</li>" %>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

                <!--Referencias-->
                <div class="modal" id='<%# "referenciasModal" + Container.ItemIndex %>' tabindex="-1">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title text-primary"><%# Eval("TitReferencias") %></h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" arial-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <ul>
                                     <%# (Eval("Referencias") as string[] ?? new string[0]).Length > 0 
                                            ? string.Join("", (Eval("Referencias") as string[]).Select(referencia => $"<li>{referencia}</li>"))
                                            : "<li>No hay referencias disponibles.</li>" %>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>

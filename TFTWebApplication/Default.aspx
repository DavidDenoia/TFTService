﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TFTWebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="text-center my-3">
        <image src="Content/imagenes/iatext-ulpgc.png" alt="ULPGC IATEXT" style="max-height: 60px; margin-right: 20px;"/>
        <img src="Content/imagenes/clariah.png" alt="CLARIAH-ES" style="max-height: 60px;" />
    </div>
    
    <!--<h1 >Conversor de Cifras a Texto en Catalán</h1>-->

    <asp:Panel ID="Panel1" runat="server" DefaultButton="botonTraducir">
    <div class="container mt-3">
        <!-- Título -->
        <div class="text-center border rounded-top py-2 bg-light">
            <strong class="text-primary">
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources: Resource, TituloPrincipal %>"></asp:Label>

            </strong>
        </div>

        <!-- Campo de búsqueda -->
        <div class="border rounded-bottom p-3 bg-white">
            <div class="input-group">
                <asp:TextBox ID="txtNumero" CssClass="form-control flex-grow-1" MaxLength="130" 
                    placeholder="<%$ Resources: Resource, PlaceHolderNumero %>" runat="server"></asp:TextBox>

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
                                data-bs-target='<%# "#referenciasModal" + Container.ItemIndex %>' title='<%# Eval("TitReferencias") %>'
                                visible='<%# !string.IsNullOrEmpty(Eval("TitReferencias") as string) %>'>
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
                      <asp:LinkButton runat="server" ID="btnLeer"
                        OnClientClick='<%# "Leer_Click(\"" + HttpUtility.JavaScriptStringEncode(string.Join(", ", (List<string>)Eval("Respuestas"))) + "\"); return false;" %>'
                        CssClass="btn btn-lg me-1">
                        <i class="bi bi-volume-up"></i>
                      </asp:LinkButton>

                       <asp:LinkButton runat="server"
                            CssClass="btn btn-lg me-1"
                            Text='<i class="bi bi-copy"></i>'
                            OnClientClick='<%# "copiarTexto(this); return false;" %>'
                            data-texto='<%# HttpUtility.JavaScriptStringEncode(string.Join(", ", ((List<string>)Eval("Respuestas")).ToArray())) %>'>
                      </asp:LinkButton>


                      <%# string.Join(", ", ((List<string>)Eval("Respuestas")).ToArray()) %>

                    </p>

                    <!--Contenido expansible-->
                    
                     <div id='<%# "content" + Container.ItemIndex %>' class="collapse">
                         

                        <!-- Valor Numérico -->
                        <%# !string.IsNullOrEmpty((string)Eval("ValorNumerico")) ? $"<h6 class=\"text-primary\">{Eval("TitValorNumerico")}: {Eval("ValorNumerico")}</h6>" : "" %>

                        <!--Opciones adicionales-->
                        <h6 class="text-primary"><%# Eval("TitOpciones") %></h6>
                        <ul>
                            <%# string.Join("", ((List<TFTWebApplication.ServiceReference1.Opcion>)Eval("MasOpciones"))
    .Select(o => $"<strong>{o.Titulo}:</strong><ul>{string.Join("", o.Opciones.Select(op => $"<li>{op}</li>"))}</ul>")) %>

                        </ul>

                        <!--Ejemplos-->
                        <h6 class="text-primary"><%# Eval("TitEjemplos") %></h6>
                        <ul>
                      
                            <%# (Eval("Ejemplos") as List<string>)?.Count > 0 
                                ? string.Join("", ((List<string>)Eval("Ejemplos")).Select(ejemplo => $"<li>{ejemplo}</li>"))
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
                                    <%# (Eval("Notas") as List<string>)?.Count > 0 
                                        ? string.Join("", ((List<string>)Eval("Notas")).Select(nota => $"<li>{nota}</li>")) 
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
                                      <%# (Eval("Referencias") as List<string>)?.Count > 0 
                                          ? string.Join("", ((List<string>)Eval("Referencias")).Select(referencia => $"<li>{referencia}</li>")) 
                                          : "<li>No hay referencias disponibles.</li>" %>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </ItemTemplate>
    </asp:Repeater>

    <!--Panel de error-->

    <asp:Panel ID="panelError" runat="server" CssClass="border border-danger rounded-top" Visible="false">

        <div class="panel-heading bg-light py-3 text-center">
             <strong class="text-danger">ERROR -</strong> <span class="text-danger"><asp:Literal runat="server" Text="<%$ Resources: Resource, FormatosAdmitidos%>"/></span>
        </div>
            
    <ul>
        <li><strong><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea1%>"/></strong></li>
        <li><strong><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea2%>"/></strong></li>
        <li><strong><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea3%>"/></strong></li>
        <li><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea4%>"/> <code>13289</code> <code>53625999567</code> <code>-345676</code></li>
        <li><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea5%>"/><code>13 289</code> <code>53 625 999 567</code> <code>-345 676</code></li>
        <li><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea6%>"/> <code>12.58</code> <code>45,78997</code> <code>-47.2</code> <code>-98,712</code></li>
        <li><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea7%>"/> <code>3/4</code> <code>78/125</code> <code>-3/4</code> <code>78/-125</code></li>
        <li><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea8.1%>"/><code>E</code><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea8.2%>"/>  <code>2,4E10</code> <code>5E-3</code> <code>-2,4E10</code> <code>-5.23E-3</code></li>
        <li><asp:Literal runat="server" Text="<%$ Resources: Resource, ErrorLinea9%>"/> <code>DLVI</code> <code>IX</code> <code>XXXVI</code></li>
        <li><asp:Literal runat="server" Text="<%$ Resources: Resource, EscribaCantidades%>"/> <strong>euros</strong> <asp:Literal runat="server" Text="<%$ Resources: Resource, ConSimbolo%>"/><code>€</code>: <code>45€</code> <code>23,78€</code></li>
        <li><asp:Literal runat="server" Text="<%$ Resources: Resource, EscribaCantidades%>"/> <strong>pesos</strong> <asp:Literal runat="server" Text="<%$ Resources: Resource, ConSimbolo%>"/><code>$</code>: <code>$1000</code> <code>123,02$</code></li>
        <li><asp:Literal runat="server" Text="<%$ Resources: Resource, EscribaCantidades%>"/> <strong><asp:Literal runat="server" Text="<%$ Resources: Resource, Dolar%>"/> </strong><asp:Literal runat="server" Text="<%$ Resources: Resource, ConSimbolo%>"/><code>$</code>: <code>428$</code> <code>$897.30</code></li>
    </ul>
    </asp:Panel>

    <!--Panel de bienvenida-->
    <asp:Panel ID="panelBienvenida" runat="server" Visible="false">
        <p><asp:Literal runat="server" Text="<%$ Resources: Resource, Bienvenida1.1%>"/>
           <asp:Literal runat="server" Text="<%$ Resources: Resource, Bienvenida1.2%>"/>
           <asp:Literal runat="server" Text="<%$ Resources: Resource, Bienvenida1.3%>"/>
            <asp:Literal runat="server" Text="<%$ Resources: Resource, Bienvenida1.4%>"/></p>

        <p><asp:Literal runat="server" Text="<%$ Resources: Resource, Bienvenida2.1%>"/>
            <asp:Literal runat="server" Text="<%$ Resources: Resource, Bienvenida2.2%>"/>
            <asp:Literal runat="server" Text="<%$ Resources: Resource, Bienvenida2.3%>"/>
        </p>

        <div class="container text-center">
            <div class="row justify-content-center">
                <h3 class="text-primary mb-3"> <asp:Literal runat="server" Text="<%$ Resources: Resource, PruebaEjemplos%>"/></h3>
            </div>
             
          <div class="d-flex flex-wrap justify-content-center gap-3 mb-2">
                <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo1" runat="server" Text="18759" OnClick="Ejemplo_Click" CommandArgument="18759"/>
                <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo2" runat="server" Text="53625947867" OnClick="Ejemplo_Click" CommandArgument="53625947867" />
                <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo3" runat="server" Text="-349996" OnClick="Ejemplo_Click" CommandArgument="-349996" />
          </div>
          <div class="d-flex flex-wrap justify-content-center gap-3 mb-2">
                <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo4" runat="server" Text="12 289" OnClick="Ejemplo_Click" CommandArgument="12 289" />
                <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo5" runat="server" Text="76 879 345 567" OnClick="Ejemplo_Click" CommandArgument="76 879 345 567"/>
                <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo6" runat="server" Text="-657 879" OnClick="Ejemplo_Click" CommandArgument="-657 879" />
          </div>
          <div class="d-flex flex-wrap justify-content-center gap-3 mb-2">
              <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo7" runat="server" Text="21.1949" OnClick="Ejemplo_Click" CommandArgument="21.1949"/>
              <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo8" runat="server" Text="1956,1959" OnClick="Ejemplo_Click" CommandArgument="1956,1959" />
              <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo9" runat="server" Text="-67.2465" OnClick="Ejemplo_Click" CommandArgument="-67.2465" />
              <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo10" runat="server" Text="-57.9" OnClick="Ejemplo_Click" CommandArgument="-57.9" />
          </div>
          <div class="d-flex flex-wrap justify-content-center gap-3 mb-2">
              <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo11" runat="server" Text="3/5" OnClick="Ejemplo_Click" CommandArgument="3/5" />
              <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo12" runat="server" Text="9874/23423" OnClick="Ejemplo_Click" CommandArgument="9874/23423" />
              <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo13" runat="server" Text="-3/5" OnClick="Ejemplo_Click" CommandArgument="-3/5" />
              <asp:Button CssClass="btn btn-outline-primary btn-sm rounded text-dark" ID="BotonEjemplo14" runat="server" Text="9874/-23423" OnClick="Ejemplo_Click" CommandArgument="9874/-23423" />
          </div>


        </div>
       
    </asp:Panel>

</asp:Content>

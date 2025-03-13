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
                        <asp:LinkButton runat="server" CommandArgument='<%# string.Join(", ", ((List<string>)Eval("Respuestas")).ToArray()) %>' 
                            OnClick="Leer_Click"
                            CssClass="btn btn-lg me-1">
                                <i class="bi bi-volume-up"></i>
                        </asp:LinkButton>

                        <asp:LinkButton runat="server" CommandArgument='<%# string.Join(", ", ((List<string>)Eval("Respuestas")).ToArray()) %>'
                            OnClick="Copiar_Click"
                            CssClass="btn btn-lg me-1">
                            <i class="bi bi-copy"></i>
                        </asp:LinkButton>

                        <strong>Respuesta:</strong> <%# string.Join(", ", ((List<string>)Eval("Respuestas")).ToArray()) %>

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

    <!--Panel de error-->

    <asp:Panel ID="panelError" runat="server" CssClass="border border-danger rounded-top" Visible="false">

        <div class="panel-heading bg-light py-3 text-center">
             <strong class="text-danger">ERROR -</strong> <span class="text-danger">Los formatos admitidos para el catalán son los siguientes:</span>
        </div>
            
    <ul>
        <li><strong>Los símbolos de monedas (£, ¥, etc.) no están admitidos para este idioma.</strong></li>
        <li><strong>Puede que el número, el exponente o la parte decimal sea demasiado grande.</strong></li>
        <li><strong>NO escriba el número con letras, excepto si es un número romano.</strong></li>
        <li>Escriba un número sin coma ni punto para los miles o millones: <code>13289</code> <code>53625999567</code> <code>-345676</code></li>
        <li>Los miles o millones se pueden separar con un espacio en blanco: <code>13 289</code> <code>53 625 999 567</code> <code>-345 676</code></li>
        <li>En los decimales puede usar un punto o una coma: <code>12.58</code> <code>45,78997</code> <code>-47.2</code> <code>-98,712</code></li>
        <li>Para escribir fracciones use la barra: <code>3/4</code> <code>78/125</code> <code>-3/4</code> <code>78/-125</code></li>
        <li>Para escribir en notación científica use la <code>E</code> sin espacios: <code>2,4E10</code> <code>5E-3</code> <code>-2,4E10</code> <code>-5.23E-3</code></li>
        <li>Si es un número romano, escríbalo en mayúscula: <code>DLVI</code> <code>IX</code> <code>XXXVI</code></li>
        <li>Escriba cantidades en <strong>euros</strong> con el símbolo <code>€</code> o la palabra <code>euros</code>: <code>45€</code> <code>23,78€</code> <code>10 euros</code></li>
        <li>Escriba cantidades en <strong>pesos</strong> con el símbolo <code>$</code> o la palabra <code>pesos</code>: <code>$1000</code> <code>123,02$</code> <code>345 pesos</code></li>
        <li>Escriba cantidades en <strong>dólares</strong> con el símbolo <code>$</code> o la palabra <code>dólares</code>: <code>428$</code> <code>$897.30</code> <code>63 dólares</code></li>
    </ul>
    </asp:Panel>

    <!--Panel de bienvenida-->
    <asp:Panel ID="panelBienvenida" runat="server" Visible="false">
        <p>Este conversor puede convertir números a letras en cardinal, números en letras ordinal, números en letras fraccionario o partitivo,
            multiplicativo, decimal, romano, colectivo, número de sílabas, nombre de polígonos y poliedros, edades y nacido. 
            El conversor ofrece información morfológica, ortográfica y gramatical de cada uno de los números convertidos a letras. 
            Además, se incluyen ejemplos que ayudan a la comprensión y buen uso.</p>

        <p>Escriba un número sin coma ni punto para los miles o millones. Los miles o millones se pueden separar con un espacio en blanco. 
            En los decimales puede usar un punto o una coma. Para escribir fracciones use la barra inclinada "/". 
            Para escribr en notación científica use la E sin espacios. Si es un número romano, escriba en mayúscula todos sus símbolos.</p>

        <div class="container text-center">
            <div class="row justify-content-center">
                <h3 class="text-primary mb-3">Prueba los ejemplos</h3>
            </div>
             
          <div class="d-flex flex-wrap justify-content-center gap-3">
                <asp:Button ID="BotonEjemplo1" runat="server" Text="18759" OnClick="Ejemplo_Click" CommandArgument="18759"/>
                <asp:Button ID="BotonEjemplo2" runat="server" Text="53625947867" OnClick="Ejemplo_Click" CommandArgument="53625947867" />
                <asp:Button ID="BotonEjemplo3" runat="server" Text="-349996" OnClick="Ejemplo_Click" CommandArgument="-349996" />
          </div>
          <div class="d-flex flex-wrap justify-content-center gap-3">
                <asp:Button ID="BotonEjemplo4" runat="server" Text="12 289" OnClick="Ejemplo_Click" CommandArgument="12 289" />
                <asp:Button ID="BotonEjemplo5" runat="server" Text="76 879 345 567" OnClick="Ejemplo_Click" CommandArgument="76 879 345 567"/>
                <asp:Button ID="BotonEjemplo6" runat="server" Text="-657 879" OnClick="Ejemplo_Click" CommandArgument="-657 879" />
          </div>
          <div class="d-flex flex-wrap justify-content-center gap-3">
              <asp:Button ID="BotonEjemplo7" runat="server" Text="21.1949" OnClick="Ejemplo_Click" CommandArgument="21.1949"/>
              <asp:Button ID="BotonEjemplo8" runat="server" Text="1956,1959" OnClick="Ejemplo_Click" CommandArgument="1956,1959" />
              <asp:Button ID="BotonEjemplo9" runat="server" Text="-67.2465" OnClick="Ejemplo_Click" CommandArgument="-67.2465" />
              <asp:Button ID="BotonEjemplo10" runat="server" Text="-57.9" OnClick="Ejemplo_Click" CommandArgument="-57.9" />
          </div>
          <div class="d-flex flex-wrap justify-content-center gap-3">
              <asp:Button ID="BotonEjemplo11" runat="server" Text="3/5" OnClick="Ejemplo_Click" CommandArgument="3/5" />
              <asp:Button ID="BotonEjemplo12" runat="server" Text="9874/23423" OnClick="Ejemplo_Click" CommandArgument="9874/23423" />
              <asp:Button ID="BotonEjemplo13" runat="server" Text="-3/5" OnClick="Ejemplo_Click" CommandArgument="-3/5" />
              <asp:Button ID="BotonEjemplo14" runat="server" Text="9874/-23423" OnClick="Ejemplo_Click" CommandArgument="9874/-23423" />
          </div>


        </div>
       
    </asp:Panel>

</asp:Content>

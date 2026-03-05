<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PRESUPUESTOS_ADM.aspx.cs" Inherits="GL___C2G.WebForm9" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2A.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">
                <h1 class="page-title">Presupuestos</h1>
                <asp:ScriptManager runat="server" ID="sm1" />

                <%--RPIMER CARD QUE MUESTRA LA SELECCION DE OBRAS Y LA CARGA DE PAGO--%>
                <div class="card">
                    <div class="card-header">

                        <div class="row g-2 align-items-end w-100">
                            <div class="col-md-6">
                                <label>Obra</label>
                                <asp:DropDownList ID="ddlObras" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlObras_SelectedIndexChanged" />
                            </div>

                            <div class="col-md-6">
                                <label>Nuevo rubro</label>
                                <div class="inline-form">
                                    <asp:TextBox ID="txtNuevoRubro" runat="server" CssClass="form-control" />
                                    <asp:Button ID="btnAgregarRubro" runat="server" Text="Agregar rubro"
                                        CssClass="btn btn-primary" OnClick="btnAgregarRubro_OnClick" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%--ACA SE INICIALIZA EL UPDATEPANEL--%>
                <asp:UpdatePanel runat="server" ID="upRubros" UpdateMode="Conditional">
                    <ContentTemplate>

                        <%--   PANEL DE MENSAJES--%>

                        <br />

                        <asp:Panel ID="pnlMsg" runat="server" Visible="false" CssClass="alert">
                            <asp:Literal ID="litMsg" runat="server" />
                        </asp:Panel>

                        <asp:Repeater ID="rptRubros" runat="server"
                            OnItemDataBound="rptRubros_ItemDataBound"
                            OnItemCommand="rptRubros_ItemCommand">
                            <ItemTemplate>
                                <div class="card etapa-card">

                                    <%--ME PERMITE HACER TOGGLE--%>
                                      <button type="button"
                                            class="card-header rubro-toggle"
                                            data-bs-toggle="collapse"
                                            data-bs-target="#rubroBody_<%# Eval("id_rubroR") %>"
                                            aria-controls="rubroBody_<%# Eval("id_rubroR") %>"
                                            aria-expanded='<%# (Container.ItemIndex == RubroOpenIndex) ? "true" : "false" %>'>
                                        <div class="rubro-head-left">
                                            <span class="pill">Rubro</span>
                                            &nbsp;<strong><%# Eval("descripcion").ToString().ToUpper() %></strong>
                                        </div>
                                        <span class="chevron" aria-hidden="true"></span>
                                    </button>

                                    <%-- ME PERMITE QUE LA TABLA SE DESPLIEGUE--%>
                                       <div id="rubroBody_<%# Eval("id_rubroR") %>" 
                                         class='<%# (Container.ItemIndex == RubroOpenIndex) ? "collapse show" : "collapse" %>'>
                                        <div class="card-body">

                                            <%--ACCIONES--%>
                                            <div class="actions d-flex flex-wrap gap-2 mb-3">
                                                <asp:TextBox ID="txtRenameRubro" runat="server" CssClass="form-control d-inline-block"
                                                    Style="width: 220px;" Text='<%# Eval("descripcion") %>' />
                                                <asp:LinkButton ID="lnkRenombrar" runat="server" CssClass="btn btn-outline-secondary"
                                                    CommandName="RenombrarRubro" CommandArgument='<%# Eval("id_rubroR") %>'>
                                                          Renombrar
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkEliminar" runat="server" CssClass="btn btn-outline-danger"
                                                    CommandName="EliminarRubro" CommandArgument='<%# Eval("id_rubroR") %>'
                                                    OnClientClick="return confirm('¿Eliminar rubro y sus presupuestos?');">
                                                          Eliminar
                                                </asp:LinkButton>
                                            </div>

                                            <%-- TABLA DE PRESUPUESTOS--%>
                                            <asp:GridView ID="gvPres" runat="server"
                                                CssClass="table table-striped table-hover grid-sm"
                                                AutoGenerateColumns="false" DataKeyNames="id_presupuesto"
                                                OnRowCommand="gvPres_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="monto_real" HeaderText="Monto ($)" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                                                    <asp:BoundField DataField="tipo_desc" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="valor" HeaderText="Valor" />
                                                    <asp:BoundField DataField="fechainicio" HeaderText="Inicio" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="fechafin" HeaderText="Fin" DataFormatString="{0:dd/MM/yyyy}" />

                                                    <asp:TemplateField HeaderText="Acciones">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBorrar" runat="server" CssClass="btn btn-sm btn-danger"
                                                                CommandName="BorrarPres" CommandArgument='<%# Eval("id_presupuesto") %>'
                                                                OnClientClick="return confirm('¿Borrar presupuesto?');">
                                                                   Borrar
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                            <div class="sep"></div>

                                            <%-- ALTA DE MATERIAL--%>
                                            <div class="inline-form">
                                                <asp:HiddenField ID="hidIdRubro" runat="server" Value='<%# Eval("id_rubroR") %>' />

                                                <asp:TextBox ID="txtDescNuevo" runat="server" CssClass="form-control"
                                                    placeholder="Descripción (obligatoria)" />

                                                <%-- Tipo (Horas/Cantidad/Peso) --%>
                                                <asp:DropDownList ID="ddlTipoNuevo" runat="server" CssClass="form-select" />

                                              <%--   Valor numérico asociado al tipo (ej: 1 horas, 1 unidad, 1 kg) --%>
                                                <asp:TextBox ID="txtValorNuevo" runat="server" CssClass="form-control"
                                                    placeholder="Valor (número > 0)" />

                                                <asp:TextBox ID="txtMontoNuevo" runat="server" CssClass="form-control"
                                                    placeholder="Monto (ej. 123456,78)" />

                                                <asp:LinkButton ID="lnkAgregarPres" runat="server" CssClass="btn btn-primary"
                                                    CommandName="AgregarPres" CommandArgument='<%# Eval("id_rubroR") %>'>
                                                         Agregar presupuesto
                                                </asp:LinkButton>

                                                <span class="text-muted ms-2">* Fecha de inicio hoy, Fin en 3 meses</span>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlObras" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnAgregarRubro" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--SCRIPT PARA HACER TOGGLE--%>
    <script>
        function toggleAll(open) {
            document.querySelectorAll('[id^="rubroBody_"]').forEach(function (el) {
                var c = bootstrap.Collapse.getOrCreateInstance(el, { toggle: false });
                open ? c.show() : c.hide();
                var btn = document.querySelector('[data-bs-target="#' + el.id + '"]');
                if (btn) btn.setAttribute('aria-expanded', open ? 'true' : 'false');
            });
        }
    </script>

</asp:Content>

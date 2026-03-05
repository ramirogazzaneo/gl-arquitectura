<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FINANZAS_ADM.aspx.cs" Inherits="GL___C2G.WebForm12" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2A.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">
                <h1 class="page-title">Pagos</h1>
                <asp:ScriptManager runat="server" ID="sm1" />

                <%--RPIMER CARD QUE MUESTRA LA SELECCION DE OBRAS Y LA CARGA DE PAGO--%>
                <div class="card">
                    <div class="card-header">
                        <div class="row g-2 w-100">
                            <div class="col-md-4">
                                <label for="<%= ddlObras.ClientID %>">Obra</label>
                                <asp:DropDownList ID="ddlObras" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlObras_SelectedIndexChanged" />
                            </div>

                            <div class="col-md-8">
                                <label>Nuevo pago</label>
                                <div class="inline-form">
                                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
                                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control"
                                        MaxLength="50" placeholder="Descripción" />
                                    <asp:TextBox ID="txtMonto" runat="server" CssClass="form-control"
                                        placeholder="Monto (ej. 12345,67)" />
                                    <label class="form-check-label">
                                        <asp:CheckBox ID="chkPagadoNuevo" runat="server" CssClass="form-check-input" />
                                        Pagado
                                    </label>
                                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar pago"
                                        CssClass="btn btn-primary" OnClick="btnAgregar_OnClick" />
                                </div>
                                <span class="text-muted d-block mt-1">La fecha no puede ser menor al día de hoy. El vencimiento será dentro de 15 días.
                                </span>
                            </div>
                        </div>
                    </div>
                </div>

                <%--ACA SE INICIALIZA EL UPDATEPANEL--%>
                <asp:UpdatePanel runat="server" ID="updPagos" UpdateMode="Conditional">
                    <ContentTemplate>

                        <%--   PANEL DE MENSAJES--%>
                        <asp:Panel ID="pnlMsg" runat="server" Visible="false" CssClass="alert" Style="margin: 12px 0;">
                            <asp:Literal ID="litMsg" runat="server" />
                        </asp:Panel>

                        <%-- LISTADO DE PAGOS--%>
                        <asp:Panel ID="pnlLista" runat="server" Visible="false">
                            <div class="card">
                                <div class="card-header"><strong>Pagos de la obra</strong></div>


                                <div class="card-body">
                                    <div class="table-responsive">

                                        <asp:GridView ID="gvPagos" runat="server"
                                            CssClass="table table-striped table-hover grid-sm"
                                            AutoGenerateColumns="false" DataKeyNames="id_pago"
                                            OnRowCommand="gvPagos_RowCommand"
                                            OnRowDataBound="gvPagos_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}">
                                                    <ItemStyle Width="110px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fecha_ven" HeaderText="Vence" DataFormatString="{0:dd/MM/yyyy}">
                                                    <ItemStyle Width="110px" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Descripción">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control form-control-sm"
                                                            MaxLength="50" Text='<%# Bind("descripcion") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="260px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Monto ($)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMontoEdit" runat="server"
                                                            CssClass="form-control form-control-sm text-end"
                                                            Text='<%# Bind("monto","{0:F2}") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Estado">
                                                    <ItemTemplate>
                                                        <div class="d-flex align-items-center gap-4">
                                                            <span class='<%# Convert.ToBoolean(Eval("estado_pago")) ? "badge bg-success" : "badge bg-warning text-dark" %>'>
                                                                <%# Convert.ToBoolean(Eval("estado_pago")) ? "Pagado" : "Pendiente" %>
                                                            </span>
                                                        </div>
                                                    </ItemTemplate>

                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Acciones">
                                                    <ItemTemplate>
                                                        <div class="nowrap">
                                                            <asp:LinkButton ID="lkGuardar" runat="server" CssClass="btn btn-sm btn-success me-1"
                                                                CommandName="Guardar" CommandArgument='<%# Eval("id_pago") %>'>Guardar</asp:LinkButton>
                                                            <asp:LinkButton ID="lkBorrar" runat="server" CssClass="btn btn-sm btn-danger"
                                                                CommandName="Borrar" CommandArgument='<%# Eval("id_pago") %>'
                                                                OnClientClick="return confirm('¿Eliminar este pago?');">Eliminar</asp:LinkButton>
                                                            <asp:LinkButton ID="btnToggle" runat="server"
                                                                CssClass="btn btn-sm btn-success me-1"
                                                                CommandName="CambiarEstado"
                                                                CommandArgument='<%# Eval("id_pago") + "|" + Eval("estado_pago") %>'>
                                                                <%# Convert.ToBoolean(Eval("estado_pago")) ? "Marcar No pago" : "Marcar pagado" %>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="180px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlObras" EventName="SelectedIndexChanged" />
                        <%--RECARGO SOLO EL UPDATEPANEL--%>
                        <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />
                        <%--RECARGO SOLO EL UPDATEPANEL--%>
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>

</asp:Content>

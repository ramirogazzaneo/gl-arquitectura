<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PRESUPUESTOS_USR.aspx.cs" Inherits="GL___C2G.WebForm10" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">
                <h1 class="page-title">Mis Presupuestos</h1>
                <asp:ScriptManager runat="server" ID="sm1" />

             
                <div class="card">
                    <div class="card-header">
                        <div class="row g-2 align-items-end w-100">
                            <div class="col-md-6">
                                <asp:Label runat="server" AssociatedControlID="ddlObras" CssClass="form-label" Text="Obra" />
                                <asp:DropDownList ID="ddlObras" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlObras_SelectedIndexChanged" />
                            </div>


                        </div>
                    </div>
                </div>

                <asp:UpdatePanel runat="server" ID="upRubros" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMsg" runat="server" CssClass="text-muted mt-3 d-block" Visible="false" />

                        <asp:Repeater ID="rptRubros" runat="server" OnItemDataBound="rptRubros_ItemDataBound">
                            <ItemTemplate>
                                <div class="card etapa-card">

                                    
                                    <button type="button"
                                        class="card-header rubro-toggle"
                                        data-bs-toggle="collapse"
                                        data-bs-target="#rubroBody_<%# Eval("id_rubroR") %>"
                                      
                                        aria-controls="rubroBody_<%# Eval("id_rubroR") %>">
                                        <div class="rubro-head-left">
                                            <strong><%# Eval("descripcion").ToString().ToUpper() %></strong>
                                        </div>
                                        <span class="chevron" aria-hidden="true"></span>
                                    </button>

                                  
                                    <div id="rubroBody_<%# Eval("id_rubroR") %>" class='collapse'>
                                        <div class="card-body">
                                            <asp:GridView ID="gvMateriales" runat="server"
                                                CssClass="table table-striped table-hover grid-sm"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                <Columns>

                                                    <asp:BoundField DataField="descripcion" HeaderText="Material"/>
                                                    <asp:BoundField DataField="tipo_desc" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="valor" HeaderText="Valor" />
                                                    <asp:BoundField DataField="fechainicio" HeaderText="Inicio" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="fechafin" HeaderText="vence" DataFormatString="{0:dd/MM/yyyy}" />

                                                   

                                                    <asp:BoundField DataField="monto_real" HeaderText="Monto ($)"
                                                        DataFormatString="{0:C2}" HtmlEncode="false">
                                                        <HeaderStyle CssClass="col-money" />
                                                        <ItemStyle CssClass="col-money" />
                                                    </asp:BoundField>

                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <em>No hay materiales en este rubro.</em>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </div>

                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlObras" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

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

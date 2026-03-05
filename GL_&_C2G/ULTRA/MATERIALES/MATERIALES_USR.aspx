<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MATERIALES_USR.aspx.cs" Inherits="GL___C2G.WebForm8" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">
                <h1 class="page-title">Materiales</h1>
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
                                        data-bs-target="#rubroBody_<%# Eval("id_rubro") %>"
                                        aria-controls="rubroBody_<%# Eval("id_rubro") %>">
                                        <div class="rubro-head-left">
                                            <strong><%# Eval("descripcion") %></strong>     
                                        </div>
                                        <span class="chevron" aria-hidden="true"></span>
                                    </button>


                                    <div id="rubroBody_<%# Eval("id_rubro") %>" class='collapse %>'>
                                        <div class="card-body">
                                            <asp:GridView ID="gvMateriales" runat="server"
                                                CssClass="table table-striped table-hover grid-sm"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                <Columns>

                                                    <asp:BoundField DataField="descripcion" HeaderText="Material">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>


                                                    <asp:TemplateField HeaderText="Entregado">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server"
                                                                Checked='<%# Convert.ToBoolean(Eval("entrega")) %>' Enabled="false" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-money" />
                                                        <ItemStyle CssClass="col-money" />
                                                    </asp:TemplateField>
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

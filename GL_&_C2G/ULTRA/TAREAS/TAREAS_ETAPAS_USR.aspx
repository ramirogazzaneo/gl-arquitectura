<%@ Page Title="Mis Etapas y Tareas" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="TAREAS_ETAPAS_USR.aspx.cs"
    Inherits="GL___C2G.AF_TAREAS_ETAPAS_USR" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">
                <h1 class="page-title">Mis Tareas y Etapas</h1>
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


                <asp:UpdatePanel runat="server" ID="upEtapas" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Repeater ID="rptEtapas" runat="server"
                            OnItemDataBound="rptEtapas_ItemDataBound">
                            <ItemTemplate>
                                <div class="card etapa-card">

                                    <button type="button"
                                        class="card-header rubro-toggle"
                                        data-bs-toggle="collapse"
                                        data-bs-target="#rubroBody_<%# Eval("id_etapa") %>"
                                        aria-controls="rubroBody_<%# Eval("id_etapa") %>">
                                        <div class="rubro-head-left">
                                            <strong><%# Eval("nombre").ToString().ToUpper() %></strong>
                                        </div>
                                        <span class="chevron" aria-hidden="true"></span>
                                    </button>


                                    <div id="rubroBody_<%# Eval("id_etapa") %>" class='collapse'>

                                        <div class="card-body">

                                            <asp:GridView ID="gvTareas" runat="server"
                                                CssClass="table table-striped table-hover grid-sm"
                                                AutoGenerateColumns="false">
                                                <Columns>

                                                    
                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                                                    <asp:BoundField DataField="fecha_inicio" HeaderText="Inicio"
                                                        DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />

                                                    <asp:TemplateField HeaderText="Fin">
                                                        <ItemTemplate>
                                                            <%# (Eval("fecha_fin") == DBNull.Value) ? "" : string.Format("{0:dd/MM/yyyy}", Eval("fecha_fin")) %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CheckBoxField DataField="es_hito" HeaderText="Hito" ReadOnly="true" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <em>No hay tareas en esta etapa.</em>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:Label ID="lblMsg" runat="server" CssClass="text-muted mt-2 d-block" Visible="false" />
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

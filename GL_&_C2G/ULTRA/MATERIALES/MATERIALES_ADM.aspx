<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MATERIALES_ADM.aspx.cs" Inherits="GL___C2G.WebForm6" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2A.css") %>" rel="stylesheet" />
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
                                <asp:Label runat="server" AssociatedControlID="ddlObras" Text="Obra" />
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
                                <asp:Label ID="lblMsgTop" runat="server" CssClass="text-danger mt-2 d-block"
                                    Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>

              
                <asp:UpdatePanel runat="server" ID="upRubros" UpdateMode="Conditional">
                    <ContentTemplate>

                      
                        <br />
                        <asp:Panel ID="pnlMsg" runat="server" Visible="false" CssClass="alert">
                            <asp:Literal ID="litMsg" runat="server" />
                        </asp:Panel>

                       
                        <asp:Repeater ID="rptRubros" runat="server"
                            OnItemDataBound="rptRubros_ItemDataBound"
                            OnItemCommand="rptRubros_ItemCommand">

                            <ItemTemplate>
                                <div class="card etapa-card">

                                   
                                    <button type="button"
                                        class="card-header rubro-toggle"
                                        data-bs-toggle="collapse"
                                        data-bs-target="#rubroBody_<%# Eval("id_rubro") %>"
                                        aria-controls="rubroBody_<%# Eval("id_rubro") %>"
                                        aria-expanded='<%# (Container.ItemIndex == RubroOpenIndex) ? "true" : "false" %>'>
                                        <div class="rubro-head-left">
                                            <span class="pill">Rubro</span>
                                            &nbsp;<strong><%# Eval("descripcion").ToString().ToUpper() %></strong>
                                        </div>
                                        <span class="chevron" aria-hidden="true"></span>
                                    </button>

                                 
                                    <div id="rubroBody_<%# Eval("id_rubro") %>"
                                        class='<%# (Container.ItemIndex == RubroOpenIndex) ? "collapse show" : "collapse" %>'>
                                        <div class="card-body">

                                           
                                            <div class="actions mb-3">
                                                <asp:TextBox ID="txtRenameRubro" runat="server"
                                                    CssClass="form-control d-inline-block" Style="width: 260px;"
                                                    Text='<%# Eval("descripcion") %>' />
                                                <asp:LinkButton ID="lnkRenombrarRubro" runat="server"
                                                    CssClass="btn btn-outline-secondary"
                                                    CommandName="RenombrarRubro"
                                                    CommandArgument='<%# Eval("id_rubro") %>'>
                                                    Renombrar
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkEliminarRubro" runat="server"
                                                    CssClass="btn btn-outline-danger"
                                                    CommandName="EliminarRubro"
                                                    CommandArgument='<%# Eval("id_rubro") %>'
                                                    OnClientClick="return confirm('¿Eliminar rubro y sus materiales?');">
                                                    Eliminar
                                                </asp:LinkButton>
                                            </div>

                                           
                                            <asp:GridView ID="gvMateriales" runat="server"
                                                CssClass="table table-striped table-hover grid-sm"
                                                AutoGenerateColumns="false"
                                                DataKeyNames="id_material,cantidad,entrega"
                                                OnRowCommand="gvMateriales_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="descripcion" HeaderText="Material" />
                                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                                                    <asp:TemplateField HeaderText="Entregado">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkEntrega" runat="server"
                                                                Checked='<%# Convert.ToBoolean(Eval("entrega")) %>'
                                                                Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Acciones">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkToggle" runat="server"
                                                                CommandName="ToggleEntrega"
                                                                CommandArgument='<%# Eval("id_material") %>'
                                                                CssClass="btn btn-sm btn-outline-secondary"
                                                                Text='<%# Convert.ToBoolean(Eval("entrega")) ? "Marcar pendiente" : "Marcar entregado" %>' />
                                                            <asp:LinkButton ID="lnkBorrarMaterial" runat="server"
                                                                CssClass="btn btn-sm btn-danger"
                                                                CommandName="BorrarMaterial"
                                                                CommandArgument='<%# Eval("id_material") %>'
                                                                OnClientClick="return confirm('¿Borrar material?');">
                                                                Borrar
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                            <div class="sep"></div>

                                         
                                            <div class="inline-form">
                                                <asp:HiddenField ID="hidIdRubro" runat="server"
                                                    Value='<%# Eval("id_rubro") %>' />
                                                <asp:TextBox ID="txtMaterial" runat="server"
                                                    CssClass="form-control d-inline-block"
                                                    Placeholder="Descripción del material" />
                                                <asp:TextBox ID="txtCantidad" runat="server"
                                                    CssClass="form-control d-inline-block"
                                                    Placeholder="Cantidad" />
                                                <label class="form-check-label">
                                                    <asp:CheckBox ID="chkEntregado" runat="server"
                                                        CssClass="form-check-input" />
                                                    Entregado
                                                </label>
                                                <asp:LinkButton ID="lnkAgregarMaterial" runat="server"
                                                    CssClass="btn btn-primary"
                                                    CommandName="AgregarMaterial"
                                                    CommandArgument='<%# Eval("id_rubro") %>'>
                                                    Agregar material
                                                </asp:LinkButton>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlObras"
                            EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnAgregarRubro"
                            EventName="Click" />
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

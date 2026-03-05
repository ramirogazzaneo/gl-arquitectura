<%@ Page Title="Tareas y Etapas (Admin)" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="TAREAS_ETAPAS_ADM.aspx.cs" Inherits="GL___C2G.AF_TAREAS_ETAPAS_ADM" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2A.css") %>" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">
                <h1 class="page-title">Tareas y Etapas</h1>

                <asp:ScriptManager runat="server" ID="sm1" />


                <div class="card">
                    <div class="card-header">
                        <div class="row g-2 align-items-end w-100">
                            <div class="col-md-6">
                                <label for="<%= ddlObras.ClientID %>">Obra</label>
                                <asp:DropDownList ID="ddlObras" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlObras_SelectedIndexChanged" />
                                <span class="text-muted" style="margin-left: .5rem;">Se listaran solo las obras en 'Proceso''</span>
                            </div>
                            <div class="col-md-6">
                                <label>Nueva etapa</label>
                                <div class="inline-form">
                                    <asp:TextBox ID="txtNuevaEtapa" runat="server" CssClass="form-control" />
                                    <asp:Button ID="btnAgregarEtapa" runat="server" Text="Agregar etapa"
                                        CssClass="btn btn-primary" OnClick="btnAgregarEtapa_OnClick" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>


                <asp:UpdatePanel runat="server" ID="upEtapas" UpdateMode="Conditional">
                    <ContentTemplate>

                        <br />

                        <asp:Panel ID="pnlMsg" runat="server" Visible="false" CssClass="alert">
                            <asp:Literal ID="litMsg" runat="server" />
                        </asp:Panel>

                        <asp:Repeater ID="rptEtapas" runat="server"
                            OnItemDataBound="rptEtapas_ItemDataBound"
                            OnItemCommand="rptEtapas_ItemCommand">
                            <ItemTemplate>


                                <div class="card etapa-card">


                                    <button type="button"
                                        class="card-header rubro-toggle"
                                        data-bs-toggle="collapse"
                                        data-bs-target="#rubroBody_<%# Eval("id_etapa") %>"
                                        aria-controls="rubroBody_<%# Eval("id_etapa") %>"
                                        aria-expanded='<%# (Container.ItemIndex == RubroOpenIndex) ? "true" : "false" %>'>
                                        <div class="rubro-head-left">
                                            <span class="pill">Etapa</span>
                                            &nbsp;<strong><%# Eval("nombre").ToString().ToUpper() %></strong>
                                        </div>

                                        <div class="rubro-head-right" style="display: flex; align-items: center; gap: .75rem;">
                                            <span class="text-muted small" style="margin-left: 900px;">
                                                <asp:Literal ID="litAvanceText" runat="server" />
                                            </span>
                                            <div class="progress" style="width: 180px; height: 10px;">
                                                <div id="barAvance" runat="server" class="progress-bar"></div>
                                            </div>
                                        </div>
                                        <span class="chevron" aria-hidden="true"></span>
                                    </button>


                                    <div id="rubroBody_<%# Eval("id_etapa") %>"
                                        class='<%# (Container.ItemIndex == RubroOpenIndex) ? "collapse show" : "collapse" %>'>
                                        <div class="card-body">

                                            
                                            <div class="row g-2 align-items-end mb-3">
                                                <div class="col-md-8">
                                                    <div class="inline-form">
                                                        <asp:TextBox ID="txtRename" runat="server" CssClass="form-control"
                                                            Placeholder="Nuevo nombre de la etapa" />
                                                        <asp:LinkButton ID="lnkRenombrar" runat="server"
                                                            CssClass="btn btn-outline-secondary"
                                                            CommandName="Renombrar"
                                                            CommandArgument='<%# Eval("id_etapa") %>'>
                                                             Renombrar
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>

                                                <div class="col-md-4 text-md-end">
                                                    <asp:LinkButton ID="lnkEliminarEtapa" runat="server"
                                                        CssClass="btn btn-outline-danger"
                                                        CommandName="EliminarEtapa"
                                                        CommandArgument='<%# Eval("id_etapa") %>'
                                                        OnClientClick="return confirm('¿Eliminar esta etapa y todas sus tareas?');">
                                                              Eliminar etapa
                                                    </asp:LinkButton>
                                                </div>
                                            </div>




                                            <asp:GridView ID="gvTareas" runat="server"
                                                CssClass="table table-striped table-hover grid-sm"
                                                AutoGenerateColumns="false" DataKeyNames="id_tarea"
                                                OnRowCommand="gvTareas_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                                                    <asp:BoundField DataField="fecha_inicio" HeaderText="Inicio"
                                                        DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="fecha_fin" HeaderText="Fin"
                                                        DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:CheckBoxField DataField="es_hito" HeaderText="Hito" />



                                                    <asp:TemplateField HeaderText="Acciones">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkFin" runat="server"
                                                                CommandName="Finalizar"
                                                                CommandArgument='<%# Eval("id_tarea") %>'
                                                                Enabled='<%# PuedeFinalizar(Eval("fecha_inicio"), Eval("fecha_fin")) %>'
                                                                CssClass='<%# ClaseBotonFinalizar(Eval("fecha_inicio"), Eval("fecha_fin")) %>'
                                                                ToolTip='<%# PuedeFinalizar(Eval("fecha_inicio"), Eval("fecha_fin")) 
                        ? "Marcar como finalizada" 
                        : "Aún no podés finalizar: la fecha de inicio es " 
                          + ((Eval("fecha_inicio") == DBNull.Value) ? "-" : ((DateTime)Eval("fecha_inicio")).ToString("dd/MM/yyyy")) %>'>
            Finalizar
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lnkDel" runat="server" CssClass="btn btn-sm btn-danger"
                                                                CommandName="Borrar" CommandArgument='<%# Eval("id_tarea") %>'
                                                                OnClientClick="return confirm('¿Borrar tarea?');">
            Borrar
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                            <div class="sep"></div>

                                            <div class="inline-form">
                                                <asp:HiddenField ID="hidIdEtapa" runat="server" Value='<%# Eval("id_etapa") %>' />
                                                <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control d-inline-block"
                                                    Placeholder="Descripción" />
                                                <asp:TextBox ID="txtIni" runat="server" CssClass="form-control d-inline-block" TextMode="Date" />
                                                <label class="form-check-label">
                                                    <asp:CheckBox ID="chkHito" runat="server" CssClass="form-check-input" />
                                                    Hito
                                                </label>
                                                <asp:LinkButton ID="lnkAgregarTarea" runat="server" CssClass="btn btn-primary"
                                                    CommandName="AgregarTarea" CommandArgument='<%# Eval("id_etapa") %>'>Agregar tarea</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlObras" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnAgregarEtapa" EventName="Click" />
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

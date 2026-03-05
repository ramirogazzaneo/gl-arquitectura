<%@ Page Title="Obras (Administrador)" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="OBRAS_ADM.aspx.cs" Inherits="GL___C2G.AE_OBRAS_ADM" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">
                <h1 class="page-title">Obras</h1>

                <asp:ScriptManager runat="server" ID="scrm" />
                <asp:UpdatePanel runat="server" ID="upGrid" UpdateMode="Conditional">
                    <ContentTemplate>

                         <div class="card card-body">

                        <asp:GridView ID="GridView1" runat="server"
                            CssClass="table table-striped table-hover grid-sm"
                            AutoGenerateColumns="False"
                            DataKeyNames="id"
                            OnRowEditing="GridView1_RowEditing"
                            OnRowCancelingEdit="GridView1_RowCancelingEdit"
                            OnRowUpdating="GridView1_RowUpdating"
                            OnRowDeleting="GridView1_RowDeleting"
                            OnRowDataBound="GridView1_RowDataBound"
                            UseAccessibleHeader="true"
                            HeaderStyle-HorizontalAlign="Left">

                            <Columns>

                                <asp:TemplateField HeaderText="Nombre">
                                    <ItemTemplate><%# Eval("nombreO") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("nombreO") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Barrio">
                                    <ItemTemplate><%# Eval("barrio") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblBarrio" runat="server" Text='<%# Eval("barrio") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Calle">
                                    <ItemTemplate><%# Eval("calle") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblCalle" runat="server" Text='<%# Eval("calle") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Localidad">
                                    <ItemTemplate><%# Eval("localidad") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblLocalidad" runat="server" Text='<%# Eval("localidad") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="m² totales">
                                    <ItemTemplate><%# Eval("metrosT") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblMetrosT" runat="server" Text='<%# Eval("metrosT") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Provincia">
                                    <ItemTemplate><%# Eval("provincia") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblProvincia" runat="server" Text='<%# Eval("provincia") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Estado">
                                    <ItemTemplate>
                                        <%# GetNivelDescripcion(Convert.ToInt32(Eval("id_estado"))) %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddEstado" runat="server" CssClass="form-select" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkTareas" runat="server"
                                            CssClass="btn btn-sm btn-primary"
                                            Text="Más información"
                                            NavigateUrl='<%# Eval("id","~/ULTRA/TAREAS/TAREAS_ETAPAS_ADM.aspx?id_obra={0}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server"
                                            CommandName="Edit" Text="Cambiar estado"
                                            CssClass="btn btn-sm btn-outline-secondary me-1" />
                                        <asp:LinkButton ID="btnDelete" runat="server"
                                            CommandName="Delete" Text="Eliminar"
                                            CssClass="btn btn-sm btn-outline-danger"
                                            OnClientClick="return confirm('¿Seguro que deseas eliminar esta obra?');" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="btnUpdate" runat="server"
                                            CommandName="Update" Text="Guardar"
                                            CssClass="btn btn-sm btn-success me-1" />
                                        <asp:LinkButton ID="btnCancel" runat="server"
                                            CommandName="Cancel" Text="Cancelar"
                                            CssClass="btn btn-sm btn-outline-secondary" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                             </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowEditing" />
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowUpdating" />
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCancelingEdit" />
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowDeleting" />
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
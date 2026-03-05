<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="USUARIOS.aspx.cs" Inherits="GL___C2G.WebForm4" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">
                <h1 class="page-title">Usuarios Activos</h1>
                <asp:ScriptManager runat="server" ID="sm1" />
                <asp:UpdatePanel runat="server" ID="upGrid" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="card card-body">

                            <asp:GridView ID="GridView1"
                                runat="server"
                                AutoGenerateColumns="False"
                                CssClass="table table-striped table-hover grid-sm"
                                OnRowEditing="GridView1_RowEditing"
                                OnRowUpdating="GridView1_RowUpdating"
                                OnRowCancelingEdit="GridView1_RowCancelingEdit"
                                OnRowDeleting="GridView1_RowDeleting"
                                OnRowDataBound="GridView1_RowDataBound"
                                DataKeyNames="id"
                                UseAccessibleHeader="true"
                                HeaderStyle-HorizontalAlign="Left">

                                <Columns>
                                    <asp:TemplateField HeaderText="Nombre">
                                        <ItemTemplate>
                                            <%# Eval("nombre") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Apellido">
                                        <ItemTemplate>
                                            <%# Eval("apellido") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblApellido" runat="server" Text='<%# Eval("apellido") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Email">
                                        <ItemTemplate>
                                            <%# Eval("email") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("email") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Teléfono">
                                        <ItemTemplate>
                                            <%# Eval("telefono") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblTelefono" runat="server" Text='<%# Eval("telefono") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Nivel">
                                        <ItemTemplate>
                                            <%# GetNivelDescripcion(Convert.ToInt32(Eval("nivel"))) %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlNivel" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkAgregarObra" runat="server"
                                                CssClass="btn btn-sm btn-primary"
                                                Text="Agregar obra"
                                                NavigateUrl='<%# Eval("id","~/ULTRA/OBRAS/DIRECCION_ADM.aspx?usuarioId={0}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>

                                            <!-- Botón editar -->
                                            <asp:LinkButton ID="btnEdit" runat="server"
                                                CommandName="Edit" Text="Cambiar Nivel"
                                                CssClass="btn btn-sm btn-outline-secondary me-1" />

                                            <!-- Botón eliminar -->
                                            <asp:LinkButton ID="btnDelete" runat="server"
                                                CommandName="Delete" Text="Eliminar"
                                                CssClass="btn btn-sm btn-outline-danger"
                                                OnClientClick="return confirm('¿Seguro que deseas eliminar este registro?');" />
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <!-- Botones cuando la fila entra en modo edición -->
                                            <asp:LinkButton ID="btnUpdate" runat="server"
                                                CommandName="Update" Text="Guardar"
                                                CssClass="btn btn-sm btn-success me-1" />

                                            <asp:LinkButton ID="btnCancel" runat="server"
                                                CommandName="Cancel" Text="Cancelar"
                                                CssClass="btn btn-sm btn-outline-light" />
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


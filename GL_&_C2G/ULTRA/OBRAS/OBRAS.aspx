<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OBRAS.aspx.cs" Inherits="GL___C2G.WebForm5" %>



<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
     <div class="canvas-light">
   <div class="sheet-card">
       <div class="page-etapas">
            <h2 class="page-title">Mis Obras</h2>
            <asp:ScriptManager runat="server" ID="scrm" />
            <asp:UpdatePanel runat="server" ID="upGrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="GridView1"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-hover"
                        DataKeyNames="id">

                        <Columns>


                            <asp:TemplateField HeaderText="nombre">
                                <ItemTemplate>
                                    <%# Eval("nombre") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblnombre" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="barrio">
                                <ItemTemplate>
                                    <%# Eval("barrio") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblbarrio" runat="server" Text='<%# Eval("barrio") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="calle">
                                <ItemTemplate>
                                    <%# Eval("calle") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblcalle" runat="server" Text='<%# Eval("calle") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="localidad">
                                <ItemTemplate>
                                    <%# Eval("localidad") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lbllocalidad" runat="server" Text='<%# Eval("localidad") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="provincia">
                                <ItemTemplate>
                                    <%# Eval("provincia") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblprovincia" runat="server" Text='<%# Eval("provincia") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="metrosT">
                                <ItemTemplate>
                                    <%# Eval("metrosT") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblmetrosT" runat="server" Text='<%# Eval("metrosT") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="descripcion">
                                <ItemTemplate>
                                    <%# Eval("descripcion") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("descripcion") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>                 

                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
 
            </asp:UpdatePanel>
        </div>
    </div>
         </div>
</asp:Content>

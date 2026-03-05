<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DOCS_ADM.aspx.cs" Inherits="GL___C2G.Z_DOCUMENTOS.WebForm1" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="canvas-light">
    <div class="sheet-card">
      <div class="page-etapas">

        <h1 class="page-title">Documentos técnicos</h1>
        <asp:ScriptManager ID="sm" runat="server" />

       <%--  ALERTA  --%>
        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert" role="alert">
          <asp:Label ID="lblAlert" runat="server" />
        </asp:Panel>

        <%-- Barra de arriba --%>
        <div class="card">
          <div class="card-body">
            <div class="row g-3 align-items-end">

              <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="ddlObras" CssClass="form-label" Text="Obra" />
                <asp:DropDownList ID="ddlObras" runat="server" CssClass="form-select"
                  AutoPostBack="true" OnSelectedIndexChanged="ddlObras_SelectedIndexChanged" />
              </div>

              <div class="col-md-5">
                <asp:Label runat="server" AssociatedControlID="txtNuevaCarpeta" CssClass="form-label" Text="Nueva carpeta" />
                <div class="input-group">
                  <asp:TextBox ID="txtNuevaCarpeta" runat="server" CssClass="form-control" MaxLength="120"
                    placeholder="Ej: Planos, Cómputo, Legal…" />
                  <asp:Button ID="btnCrearCarpeta" runat="server" CssClass="btn btn-primary"
                    Text="Agregar carpeta" OnClick="btnCrearCarpeta_Click" />
                </div>
              </div>

              <div class="col-md-3">
                <asp:Label runat="server" CssClass="form-label d-block" Text="Estado" />
                <asp:Label ID="lblMsgTop" runat="server" CssClass="text-muted small"></asp:Label>
              </div>

            </div>
          </div>
        </div>

       <%--  establezco el hf para mantener la carpeta abeirta --%>
        <asp:HiddenField ID="hfOpenCarpetaId" runat="server" />

        <%-- panel acordein --%>
        <asp:Panel ID="pnlAcordeon" runat="server" Visible="false" CssClass="mt-3">
          <div class="accordion" id="accCarpetas">
            <asp:Repeater ID="rptCarpetas" runat="server"
                          OnItemCommand="rptCarpetas_ItemCommand"
                          OnItemDataBound="rptCarpetas_ItemDataBound">
              <ItemTemplate>

                <div class="accordion-item">

                
                  <h2 class="accordion-header" id='<%# "carp_" + Eval("id_carpeta") %>'>
                    <div class="d-flex align-items-center gap-2">

                      <button class="accordion-button collapsed flex-grow-1" type="button"
                              data-bs-toggle="collapse"
                              data-bs-target='<%# "#col_" + Eval("id_carpeta") %>'
                              aria-expanded="false"
                              aria-controls='<%# "col_" + Eval("id_carpeta") %>'>
                        <span class="badge bg-light text-dark me-2">Carpeta</span>
                        <span class="fw-semibold"><%# Eval("descripcion") %></span>
                      </button>

                      <asp:TextBox ID="txtNombreCarpeta" runat="server"
                                   CssClass="form-control form-control-sm w-auto"
                                   Text='<%# Eval("descripcion") %>' />

                      <asp:LinkButton ID="btnRenombrar" runat="server"
                                      CssClass="btn btn-outline-secondary btn-sm"
                                      CommandName="rename"
                                      CommandArgument='<%# Eval("id_carpeta") %>'
                                      OnClientClick="event.stopPropagation ? event.stopPropagation() : (window.event.cancelBubble=true);"
                                      Text="Renombrar" />

                      <asp:LinkButton ID="btnEliminar" runat="server"
                                      CssClass="btn btn-outline-danger btn-sm"
                                      CommandName="del_carp"
                                      CommandArgument='<%# Eval("id_carpeta") %>'
                                      OnClientClick="event.stopPropagation ? event.stopPropagation() : (window.event.cancelBubble=true); return confirm('¿Eliminar la carpeta? Debe estar vacía.');"
                                      Text="Eliminar" />
                    </div>
                  </h2>

                <%--   COLLAPSE --%>
                  <div id='<%# "col_" + Eval("id_carpeta") %>'
                       class="accordion-collapse collapse"
                       aria-labelledby='<%# "carp_" + Eval("id_carpeta") %>'
                       data-bs-parent="#accCarpetas">
                    <div class="accordion-body">

                     <%--  documentos --%>
                      <asp:GridView ID="gvDocs" runat="server"
                                    CssClass="table table-striped table-hover"
                                    AutoGenerateColumns="False"
                                    DataKeyNames="id_documento"
                                    OnRowCommand="gvDocs_RowCommand">
                        <Columns>
                          <asp:BoundField DataField="archivo" HeaderText="Nombre" />
                          <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                          <asp:BoundField DataField="versionn" HeaderText="Versión" />
                          <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                          <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                          <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                              <asp:HyperLink ID="lnkVer" runat="server"
                                NavigateUrl='<%# "~/Handlers/VerDocumento.ashx?id=" + Eval("id_documento") %>'
                                CssClass="btn btn-sm btn-primary me-1" Text="Ver/Descargar" Target="_blank" />
                              <asp:LinkButton ID="btnEliminarDoc" runat="server"
                                CommandName="del_doc" CommandArgument='<%# Eval("id_documento") %>'
                                CssClass="btn btn-sm btn-outline-danger"
                                OnClientClick="return confirm('¿Eliminar este documento?');" Text="Eliminar" />
                            </ItemTemplate>
                          </asp:TemplateField>
                        </Columns>
                      </asp:GridView>

                      <%-- Alta de documento dentro de la carpeta --%>
                      <div class="row g-3 align-items-end mt-2">
                        <div class="col-md-2">
                          <asp:Label runat="server" AssociatedControlID="txtTipo" CssClass="form-label" Text="Tipo" />
                          <asp:TextBox ID="txtTipo" runat="server" CssClass="form-control" Text="PDF" Enabled="false" />
                        </div>

                        

                        <div class="col-md-6">
                          <asp:Label runat="server" AssociatedControlID="fuPdf" CssClass="form-label" Text="Archivo (.pdf)" />
                          <asp:FileUpload ID="fuPdf" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-2">
                          <%-- Botón normal FileUpload --%>
                          <asp:Button ID="btnSubir" runat="server" Text="Subir PDF"
                                      CssClass="btn btn-success w-100"
                                      CommandName="upload"
                                      CommandArgument='<%# Eval("id_carpeta") %>' />
                        </div>
                      </div>

                      <asp:Label ID="lblMsgRow" runat="server" CssClass="text-muted small d-block mt-2"></asp:Label>

                    </div>
                  </div>

                </div>
              </ItemTemplate>
            </asp:Repeater>
          </div>
        </asp:Panel>

        <asp:Label ID="lblMsg" runat="server" CssClass="text-muted small d-block mt-2"></asp:Label>

      </div>
    </div>
  </div>

<%--   Script: reabrir la carpeta guardada en hfOpenCarpetaId --%>
  <script>
    document.addEventListener('DOMContentLoaded', function () {
      var hf = document.getElementById('<%= hfOpenCarpetaId.ClientID %>');
      if (!hf) return;
      var id = hf.value;
      if (!id) return;

      var el = document.getElementById('col_' + id);
      if (!el) return;

      if (window.bootstrap && bootstrap.Collapse) {
        new bootstrap.Collapse(el, { toggle: true });
      } else {
        el.classList.add('show');
        var btn = document.querySelector('button[data-bs-target="#col_' + id + '"]');
        if (btn) { btn.classList.remove('collapsed'); btn.setAttribute('aria-expanded', 'true'); }
      }
    });
  </script>
</asp:Content>

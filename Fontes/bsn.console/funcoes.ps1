function GetAlvo ([string]$nomeSite, [string]$idAlvo)   
{
   .\bsn sqlite -tabela alvo -site $nomeSite -id $idAlvo | ConvertFrom-Csv
}

function GetAnuncio ([string]$nomeSite, [string]$idAlvo)   
{
   .\bsn sqlite -tabela anuncio -site $nomeSite -id $idAlvo | ConvertFrom-Csv
}

function GetSite ([string]$nomeSite)
{
	.\bsn sqlite -tabela site -site $nomeSite | ConvertFrom-Csv 
}

Function ShowHtml ([string]$nomeSite, [string]$idAlvo)   
{
   GetAlvo $nomeSite $idAlvo | Out-File .\a.htm
   .\a.htm
}

function OpenLink ([string]$nomeSite, [string]$idAlvo)
{
	start-process (GetAlvo $nomeSite $idAlvo).LinkVisitado
}

#############

function BuscarAlvo ([string]$nomeSite) {
   .\bsn sqlite -tabela alvo -site $nomeSite -ativo | .\bsn buscar -proxy -delay 20 -debug | .\bsn sqlite -alterar
}
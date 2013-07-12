import banco
import classes

def atualizarHtml(site, id_):
	alvo = classes.Alvo(banco.alvoRow(site, id_))
	alvo.atualizarHtml()
	banco.alvoUpdate(alvo)

def extrairAnuncio(cdSite, id_):
	site = classes.Site(banco.siteRow(cdSite))
	alvo = classes.Alvo(banco.alvoRow(cdSite, id_))
	return site.getAnuncio(alvo)



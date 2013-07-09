import banco
import classes

def atualizarHtml(site, id_):
	alvo = classes.Alvo(banco.alvoRow(site, id_))
	alvo.atualizarHtml()
	banco.alvoUpdate(alvo)

def extrairAnuncio(site, id_):
	return "em construcao"

def extrairCampo():
	return "em construcao"

import banco
import classes

def atualizarHtml(site, id_):
	alvo = classes.Alvo(banco.alvoRow(site, id_))
	alvo.atualizarHtml()
	banco.alvoUpdate(alvo)

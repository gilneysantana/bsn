import banco
import classes
import re

def atualizarHtml(site, id_):
	alvo = classes.Alvo(banco.alvoRow(site, id_))
	alvo.atualizarHtml()
	banco.alvoUpdate(alvo)

def extrairAnuncio(site, id_):
	return "em construcao"

def extrairCampo(site, id_, rgx):
	alvo = classes.Alvo(banco.alvoRow(site, id_))
	return re.search(rgx, alvo.retornoRequisicao)
## <span[^>]*>Bairro</span>:([\d\w\s]*)<br>

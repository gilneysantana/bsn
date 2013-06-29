import requests 
import banco


def buscar(url):
	r = requests.get(url)
	return r.text

def atualizarHtml(site, id_):
	alvo = banco.alvoRow(site, id_)
	html = buscar(alvo['linkVisitado'])
	return html


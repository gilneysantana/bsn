import banco

def atualizarHtml(site, id_):
	alvo = Alvo(banco.alvoRow(site, id_))
	alvo.atualizarHtml()
	banco.updateAlvo(alvo)

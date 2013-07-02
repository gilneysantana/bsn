import requests 

class Alvo:

	def __init__(self, row = None):
		self.retornoRequisicao = 'vazio'

		if row is not None:
			self.row = row
			self.cdSite = row['site_cd_site']
			self.retornoRequisicao = row['retornoRequisicao']
			self.linkVisitado = row['linkVisitado']

	def getRow(self):
		self.row['site_cd_site'] = self.cdSite
		self.row['retornoRequisicao'] = self.retornoRequisicao
		self.row['linkVisitado'] = self.linkVisitado
		return self.row

	def getPk(self):
		return self.cdSite + ' + id '

	def atualizarHtml(self):
		self.retornoRequisicao = requests.get(self.linkVisitado).text
